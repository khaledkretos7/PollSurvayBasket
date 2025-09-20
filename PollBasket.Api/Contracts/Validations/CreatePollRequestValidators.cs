using FluentValidation;

namespace PollBasket.Api.Contracts.Validations;

public class CreatePollRequestValidators : AbstractValidator<CreatePollRequest>
{
    public CreatePollRequestValidators()
    {
        RuleFor(x=>x.Description).NotEmpty().WithMessage("adkaj");
    }
}
