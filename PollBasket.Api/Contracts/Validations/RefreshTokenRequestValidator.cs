using Azure.Core;
using PollBasket.Api.Contracts.Authentication;
using PollBasket.Api.Entities;
namespace PollBasket.Api.Contracts.Validations;

public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
{
    public RefreshTokenRequestValidator()
    {
        RuleFor(p => p.Token).NotEmpty();
        RuleFor(p => p.RefreshToken).NotEmpty();
    }
}