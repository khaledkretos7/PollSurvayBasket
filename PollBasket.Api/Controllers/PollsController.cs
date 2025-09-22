
namespace PollBasket.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class PollsController(IPollServices pollServices) : ControllerBase
{
    private readonly IPollServices _PollServices=pollServices;
    [HttpGet("")]
    public async Task<IActionResult> GetAll() 
    {
        var polls =await _PollServices.GetAllAsync();
        var response = polls.Adapt<IEnumerable<PollResponse>>();
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult>Get(int id)
    {
        var currentPoll=await _PollServices.GetByIdAsync(id);
        var response=currentPoll.Adapt< PollResponse>();
        return currentPoll is null ? NotFound() : Ok(response);
    }

    [HttpPost("")]
    public async Task<IActionResult> Add(PollRequest request, CancellationToken cancellationToken = default!)
    {

        var newpoll =await _PollServices.AddAsync(request.Adapt<Poll>(),cancellationToken);

        return CreatedAtAction(nameof(Get), new { id = newpoll.Id }, newpoll.Adapt<PollResponse>());
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute]int id,[FromBody]PollRequest request,CancellationToken cancellationToken=default!)
    {
        var isUpdated =await _PollServices.UpdateAsync(id,request.Adapt<Poll>(),cancellationToken);

        if (!isUpdated)
            return NotFound();

        return Ok(isUpdated);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id,CancellationToken cancellationToken)
    {
        var isdeleted =await _PollServices.DeleteAsync(id,cancellationToken);

        if (!isdeleted)
            return NotFound();

        return Ok();
    }
}
