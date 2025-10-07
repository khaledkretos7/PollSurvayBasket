using PollBasket.Api.Abstractions;

namespace PollBasket.Api.Errors;

public class PollErrors
{
    public static readonly Error PollNotFound =
        new("Poll.NotFound", "No poll was found with the given ID",StatusCodes.Status404NotFound);
    public static readonly Error DublicatedPollTitle =
     new("Poll.DublicatedPollTitle", "the poll is already exists", StatusCodes.Status409Conflict);
}
