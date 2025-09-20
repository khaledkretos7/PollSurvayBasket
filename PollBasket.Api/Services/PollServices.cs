using PollBasket.Api.Entities;

namespace PollBasket.Api.Services;

public class PollServices : IPollServices
{
    private static readonly List<Poll> _polls = [
        new Poll
        {
            Id = 1,
            Title = "Title -1",
            Description="the description"
        }
        ];

    public Poll add(Poll poll)
    {
        poll.Id = _polls.Count + 1;
       _polls.Add(poll);
        return poll;
    }

    public bool Delete(int id)
    {
        var currentPoll = GetById(id);
        if (currentPoll is null)
            return false;

       _polls.Remove(currentPoll);
        return true;
    }

    public IEnumerable<Poll> GetAll()=> _polls;

    public Poll? GetById(int id)=> _polls.SingleOrDefault(x=>x.Id==id);

    public bool Update(Poll poll)
    {
        var currentPoll = GetById(poll.Id);
        if (poll is null) 
            return false;

        currentPoll.Description = poll.Description;
        currentPoll.Title = poll.Title;

        return true;
    }
}
