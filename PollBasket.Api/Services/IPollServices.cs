using PollBasket.Api.Entities;

namespace PollBasket.Api.Services;

public interface IPollServices
{
    Task<IEnumerable<Poll>>GetAllAsync();
   Task<Poll>? GetByIdAsync(int id);
   Task<Poll> AddAsync(Poll poll,CancellationToken cancellationToken=default!);
    public Task<bool> UpdateAsync(int id, Poll poll, CancellationToken cancellationToken = default!);
    public Task<bool> DeleteAsync(int id,CancellationToken cancellationToken=default!);

}
