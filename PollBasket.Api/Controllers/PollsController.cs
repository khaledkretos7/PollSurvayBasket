
namespace PollBasket.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class PollsController(IPollServices pollServices) : ControllerBase
{
    private readonly IPollServices _PollServices=pollServices;
    [HttpGet("")]
    public IActionResult GetAll() 
    {
        var polls = _PollServices.GetAll();
        var response = polls.Adapt<IEnumerable<PollResponse>>();
        return Ok(response);
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var currentPoll=_PollServices.GetById(id);
        var response=currentPoll.Adapt<PollResponse>();
        return currentPoll is null ? NotFound() : Ok(response);
    }

    [HttpPost("")]
    public IActionResult Add(CreatePollRequest request) 
    {

        var newpoll = _PollServices.add(request.Adapt<Poll>());

        return CreatedAtAction(nameof(Get), new { id = newpoll.Id }, newpoll.Adapt<PollResponse>());
    }
    [HttpPut("")]
    public IActionResult Update(CreatePollRequest request) 
    {
        var isUpdated=_PollServices.Update(request.Adapt<Poll>());

        if(!isUpdated)
            return NotFound();

        return Ok(isUpdated);
    }
    [HttpDelete("{id}")]
    public IActionResult Delete(int id) 
    {
        var isdeleted = _PollServices.Delete(id);

        if (!isdeleted)
            return NotFound();

        return Ok();
    }
}
