
using Microsoft.AspNetCore.Authorization;
using PollBasket.Api.Abstractions;

namespace PollBasket.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PollsController(IPollServices pollServices) : ControllerBase
{
    private readonly IPollServices _PollServices = pollServices;
    [HttpGet("")]
    public async Task<IActionResult> GetAll()
    {
        var polls = await _PollServices.GetAllAsync();
        var response = polls.Adapt<IEnumerable<PollResponse>>();
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _PollServices.GetAsync(id);
        return result.IsSuccess
           ? Ok(result.Value)
           : Problem();
    }
    

    [HttpPost("")]
    public async Task<IActionResult> Add(PollRequest request, CancellationToken cancellationToken = default!)
    {

        var newPoll = await _PollServices.AddAsync(request, cancellationToken);

        return CreatedAtAction(nameof(Get), new { id = newPoll.id }, newPoll); ;
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] PollRequest request, CancellationToken cancellationToken = default!)
    {
        var result = await _PollServices.UpdateAsync(id, request, cancellationToken);
        return result.IsSuccess ? NoContent() : NotFound(result.Error);

    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await _PollServices.DeleteAsync(id, cancellationToken);

        return result.IsSuccess ? NoContent() : Problem();
    }
    [HttpPut("{id}/togglePublish")]
    public async Task<IActionResult> TogglePublish([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _PollServices.TogglePublishStatusAsync(id, cancellationToken);

        return result.IsSuccess ? NoContent() : Problem();
    }
}