using PollBasket.Api.Entities;

namespace PollBasket.Api.Services;

public interface IPollServices
{
    IEnumerable<Poll>GetAll();
    Poll? GetById(int id);
    Poll add(Poll poll);
    public bool Update(Poll poll);
    public bool Delete(int id);

}
