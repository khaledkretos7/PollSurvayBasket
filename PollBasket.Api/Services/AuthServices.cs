using Microsoft.AspNetCore.Identity;
using PollBasket.Api.Abstractions;
using PollBasket.Api.Authentication;
using PollBasket.Api.Contracts.Authentication;
using PollBasket.Api.Entities;
using PollBasket.Api.Errors;
using System.Security.Cryptography;
using System.Xml.Linq;
using static Azure.Core.HttpHeader;

namespace PollBasket.Api.Services;

public class AuthServices(UserManager<ApplicationUser> user, IJwtProvider jwtProvider) : IAuthServices
{
    private readonly UserManager<ApplicationUser> _userManager = user;
    private readonly IJwtProvider _jwtProvider = jwtProvider;
    private readonly int _refreshTokenExpirationDays = 14;

    public async Task<Result<AuthResponse>> GenerateTokenAsync(string Email, string Password, CancellationToken cancellationToken = default)
    {
        //check user 
        var user = await _userManager.FindByEmailAsync(Email);
        if (user is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);
        
        //check password 
        var isValidPassword = await _userManager.CheckPasswordAsync(user, Password);
        if (!isValidPassword)
            return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);


        var (token, expiresIn) = _jwtProvider.GenerateTokenAsync(user);

        var refreshToken = GenerateRefreshToken;
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpirationDays);
        user.RefreshTokens.Add(new RefreshToken
        {
            Token = refreshToken,
            ExpireOn = refreshTokenExpiration
        });
        await _userManager.UpdateAsync(user);
        var respose= new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, token, expiresIn * 60, refreshToken, refreshTokenExpiration);
        return Result.Success(respose);
    }

    public async Task<Result<AuthResponse>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
    {
        var userId = _jwtProvider.ValidateToken(token);
        if (userId is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidJwtToken);
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidJwtToken);
        var storedRefreshToken = user.RefreshTokens.FirstOrDefault(x => x.Token == refreshToken && x.IsActive);

        if (storedRefreshToken is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidRefreshToken);
        storedRefreshToken.RevokedOn = DateTime.UtcNow;

        var (newToken, expiresIn) = _jwtProvider.GenerateTokenAsync(user);

        var newRefreshToken = GenerateRefreshToken;
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpirationDays);
        user.RefreshTokens.Add(new RefreshToken
        {
            Token = newRefreshToken,
            ExpireOn = refreshTokenExpiration
        });
        await _userManager.UpdateAsync(user);
        var response= new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, newToken, expiresIn, newRefreshToken, refreshTokenExpiration);
        return Result.Success(response);
    }

    public async Task<Result> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
    {

        var userId = _jwtProvider.ValidateToken(token);
        if (userId is null)
            Result.Failure(UserErrors.InvalidJwtToken);

        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
            Result.Failure(UserErrors.InvalidJwtToken);

        var storedRefreshToken = user.RefreshTokens.FirstOrDefault(x => x.Token == refreshToken && x.IsActive);
        if (storedRefreshToken is null)
            Result.Failure(UserErrors.InvalidRefreshToken);

        storedRefreshToken.RevokedOn = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);
        return Result.Success();
    }

  

    private static string GenerateRefreshToken => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
}
