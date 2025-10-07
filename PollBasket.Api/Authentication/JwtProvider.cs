
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PollBasket.Api.Authentication;

public class JwtProvider(IOptions<JwtOptions> JwtOptions) : IJwtProvider
{
    private readonly IOptions<JwtOptions> _Options=JwtOptions;
    public (string token, int expiresIn) GenerateTokenAsync(ApplicationUser user)
    {
        Claim[] claims = [
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new(JwtRegisteredClaimNames.GivenName, user.FirstName),
            new(JwtRegisteredClaimNames.FamilyName, user.LastName),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            ];
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_Options.Value.Key));

        var singingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        //
        var expiresIn = 30;

        var token = new JwtSecurityToken(
            issuer: _Options.Value.Issuer,
            audience: _Options.Value.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_Options.Value.DurationInMinutes),
            signingCredentials: singingCredentials
        );
         
        return (token: new JwtSecurityTokenHandler().WriteToken(token), expiresIn: _Options.Value.DurationInMinutes * 60);
    }

    public string? ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.
        GetBytes(_Options.Value.Key));
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _Options.Value.Issuer,
                ValidAudience = _Options.Value.Audience,
                IssuerSigningKey = symmetricSecurityKey
            }, out SecurityToken validationToken);
            var jwtToken = (JwtSecurityToken)validationToken;
            return jwtToken.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Sub)?.Value;
        }
        catch
        {
            return null;
        }

    }
}