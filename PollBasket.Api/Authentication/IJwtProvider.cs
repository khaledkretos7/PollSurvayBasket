namespace PollBasket.Api.Authentication;

public interface IJwtProvider
{
   public (string token, int expiresIn) GenerateTokenAsync(ApplicationUser user);
    string? ValidateToken(string token);

}
