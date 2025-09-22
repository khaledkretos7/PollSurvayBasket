using Microsoft.EntityFrameworkCore;
using PollBasket.Api.Entities;
using PollBasket.Api.Persistence;

namespace PollBasket.Api.Services;

public class PollServices(ApplicationDbContext context) : IPollServices
{
    private readonly ApplicationDbContext _context=context;

 
    public async Task<IEnumerable<Poll>> GetAllAsync() =>
        await _context.polls.AsNoTracking().ToListAsync();

    public async Task<Poll> GetByIdAsync(int id) =>
          await _context.polls.FindAsync(id);

    public async Task<Poll> AddAsync(Poll poll, CancellationToken cancellationToken = default!)
    {
        await _context.AddAsync(poll);
        await _context.SaveChangesAsync(cancellationToken);
        return poll;
    }

    public async Task<bool> UpdateAsync(int id,Poll poll, CancellationToken cancellationToken = default)
    {
        var curr=await GetByIdAsync(id);

        if (curr is null) 
            return false;

        curr.Title = poll.Title;
        curr.Summary = poll.Summary;
        curr.EndsAt = poll.EndsAt;
        curr.StartsAt = poll.StartsAt;
          
        return true;  
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var isDelted = await GetByIdAsync(id);
        if(isDelted is null)
            return false;

         _context.Remove(isDelted);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
