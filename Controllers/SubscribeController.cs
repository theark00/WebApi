
using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SubscribeController(ApiContext context) : ControllerBase
{
    private readonly ApiContext _context = context;

    [HttpPost]
    public async Task<IActionResult> Subscribe(string email)
    {
        if (ModelState.IsValid)
        {
            if (await _context.Subscribers.AnyAsync(x => x.Email == email))
                return Conflict();

            _context.Add(new SubscribersEntity { Email = email });
            await _context.SaveChangesAsync();
            return Ok();
        }


        return BadRequest();
    }

    [HttpDelete]
    public async Task<IActionResult> Unsubscribe(string email)
    {

        if (ModelState.IsValid)
        {
            var subscriberEntity = await _context.Subscribers.FirstOrDefaultAsync(x => x.Email == email);
            if (subscriberEntity == null)
                return NotFound();

            _context.Remove(subscriberEntity);
            await _context.SaveChangesAsync();

            return Ok();
        }


        return BadRequest();
    }
}
