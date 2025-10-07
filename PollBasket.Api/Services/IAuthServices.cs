using PollBasket.Api.Abstractions;
using PollBasket.Api.Contracts.Authentication;
using PollBasket.Api.Entities;

namespace PollBasket.Api.Services;

public interface IAuthServices
{
    Task<Result<AuthResponse>> GenerateTokenAsync(string Email,string Password ,CancellationToken cancellationToken=default);
    Task<Result<AuthResponse>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default);
    Task<Result> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default);
}
