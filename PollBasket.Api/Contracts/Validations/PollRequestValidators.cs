using FluentValidation;

namespace PollBasket.Api.Contracts.Validations;

public class PollRequestValidators : AbstractValidator<PollRequest>
{
    public PollRequestValidators()
    {
        RuleFor(x=>x.Title)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x=>x.Summary)
            .NotEmpty()
            .MaximumLength(1500);
        
        RuleFor(x=>x.StartsAt)
            .NotEmpty()
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today));

        RuleFor(x => x)
            .Must(hasValidDate);
    }
    private bool hasValidDate(PollRequest request)
    {
        return request.StartsAt < request.EndsAt;
    }
}