using FluentValidation;
using Microsoft.AspNetCore.Identity.Data;

namespace PollBasket.Api.Contracts.Validations;

public class LoginRequestValidators: AbstractValidator<LoginRequest>
{
    public LoginRequestValidators()
    {
        RuleFor(x=>x.Email)
            .EmailAddress()
            .NotEmpty();

        RuleFor(x=>x.Password)
            .NotEmpty();
    }
}