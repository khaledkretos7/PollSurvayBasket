using PollBasket.Api.Abstractions;
using PollBasket.Api.Contracts.Authentication;
using LoginRequest = PollBasket.Api.Contracts.Authentication.LoginRequest;

namespace PollBasket.Api.Controllers;
[Route("[controller]")]
[ApiController]
public class AuthController(IAuthServices authServices) : ControllerBase
{
    private readonly IAuthServices _authServices = authServices;

    [HttpPost("")]
    public async Task<IActionResult> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        var authResult = await _authServices.GenerateTokenAsync(request.Email, request.Password, cancellationToken);

        return authResult.IsSuccess ? Ok(authResult.Value) : Problem();
    }

    [HttpPost("refresh")]
     public async Task<IActionResult> RefreshAsync(RefreshTokenRequest refreshTokenRequest, CancellationToken cancellationToken)
    {
        var authResult = await _authServices.GetRefreshTokenAsync(refreshTokenRequest.Token, refreshTokenRequest.RefreshToken, cancellationToken);
        return authResult.IsSuccess ? Ok(authResult.Value) : Problem();
    }

    [HttpPost("revoke-refresh-token")]
     public async Task<IActionResult> RevokeRefreshAsync(RefreshTokenRequest refreshTokenRequest, CancellationToken cancellationToken)
    {
        var authResult = await _authServices.RevokeRefreshTokenAsync(refreshTokenRequest.Token, refreshTokenRequest.RefreshToken, cancellationToken);
        return authResult.IsSuccess ? Ok() : Problem();
    }
}