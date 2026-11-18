using GrowBiss.SupportApi.Models;
using GrowBiss.SupportApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace GrowBiss.SupportApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class TicketsController : ControllerBase
{
    private readonly ITicketStore _ticketStore;

    public TicketsController(ITicketStore ticketStore)
    {
        _ticketStore = ticketStore;
    }

    // GET: api/tickets
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<SupportTicket>), StatusCodes.Status200OK)]
    public ActionResult<IReadOnlyCollection<SupportTicket>> GetAll()
    {
        return Ok(_ticketStore.GetAll());
    }

    // GET: api/tickets/{id}
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(SupportTicket), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<SupportTicket> GetById(Guid id)
    {
        var ticket = _ticketStore.GetById(id);

        if (ticket is null)
        {
            return NotFound(new
            {
                message = $"Ticket with ID {id} was not found."
            });
        }

        return Ok(ticket);
    }

    // POST: api/tickets
    [HttpPost]
    [ProducesResponseType(typeof(SupportTicket), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<SupportTicket> Create(CreateTicketRequest request)
    {
        var ticket = _ticketStore.Create(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = ticket.Id },
            ticket);
    }

    // PUT: api/tickets/{id}
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(SupportTicket), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<SupportTicket> Update(
        Guid id,
        UpdateTicketRequest request)
    {
        var ticket = _ticketStore.Update(id, request);

        if (ticket is null)
        {
            return NotFound(new
            {
                message = $"Ticket with ID {id} was not found or changed concurrently."
            });
        }

        return Ok(ticket);
    }

    // DELETE: api/tickets/{id}
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(Guid id)
    {
        var deleted = _ticketStore.Delete(id);

        if (!deleted)
        {
            return NotFound(new
            {
                message = $"Ticket with ID {id} was not found."
            });
        }

        return NoContent();
    }
}