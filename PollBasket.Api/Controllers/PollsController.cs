using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PollBasket.Api.Entities;
using PollBasket.Api.Services;

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
        return Ok(polls);
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var currentPoll=_PollServices.GetById(id);
        return currentPoll is null ? NotFound() : Ok(currentPoll);
    }

    [HttpPost("")]
    public IActionResult Add(Poll request) 
    {
        var newpoll = _PollServices.add(request);

        return CreatedAtAction(nameof(Get), new { id = newpoll.Id }, newpoll);
    }
    [HttpPut("")]
    public IActionResult Update(Poll request) 
    {
        var isUpdated=_PollServices.Update(request);

        if(!isUpdated)
            return NotFound();

        return Ok(isUpdated);
    }
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var isUpdated = _PollServices.Delete(id);

        if (!isUpdated)
            return NotFound();

        return Ok();
    }
}
