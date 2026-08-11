using HelpDesk.Api.DTOs;
using HelpDesk.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.Api.Controllers;

/// <summary>
/// REST endpoints for Help Desk tickets. Thin by design — no business logic
/// or data access here, only HTTP concerns (status codes, routing, model
/// validation). All real work is delegated to ITicketService.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class TicketController : ControllerBase
{
    private readonly ITicketService _ticketService;
    private readonly ILogger<TicketController> _logger;

    public TicketController(ITicketService ticketService, ILogger<TicketController> logger)
    {
        _ticketService = ticketService;
        _logger = logger;
    }

    /// <summary>Get all tickets.</summary>
    /// <response code="200">Returns the list of tickets.</response>
    [HttpGet("All")]
    [ProducesResponseType(typeof(List<TicketDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<TicketDto>>> GetAll()
    {
        var tickets = await _ticketService.GetAllTicketsAsync();
        return Ok(tickets);
    }

    /// <summary>Get a single ticket by id.</summary>
    /// <response code="200">The ticket was found.</response>
    /// <response code="404">No ticket exists with that id.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(TicketDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TicketDto>> GetById(int id)
    {
        var ticket = await _ticketService.GetTicketByIdAsync(id);
        return Ok(ticket);
    }

    /// <summary>Create a new ticket. Status is always set to "Open".</summary>
    /// <response code="201">The ticket was created.</response>
    /// <response code="400">The request body failed validation.</response>
    [HttpPost]
    [ProducesResponseType(typeof(TicketDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TicketDto>> Create([FromBody] TicketCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("CreateTicket rejected due to invalid model state.");
            return ValidationProblem(ModelState);
        }

        var created = await _ticketService.CreateTicketAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>Update an existing ticket.</summary>
    /// <response code="200">The ticket was updated.</response>
    /// <response code="400">The request body failed validation.</response>
    /// <response code="404">No ticket exists with that id.</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(TicketDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TicketDto>> Update(int id, [FromBody] TicketUpdateDto dto)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("UpdateTicket {TicketId} rejected due to invalid model state.", id);
            return ValidationProblem(ModelState);
        }

        var updated = await _ticketService.UpdateTicketAsync(id, dto);
        return Ok(updated);
    }

    /// <summary>Delete a ticket.</summary>
    /// <response code="204">The ticket was deleted.</response>
    /// <response code="404">No ticket exists with that id.</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        await _ticketService.DeleteTicketAsync(id);
        return NoContent();
    }

    /// <summary>Get all tickets matching a status (Open, In Progress, Closed).</summary>
    /// <response code="200">Returns the matching tickets (possibly empty).</response>
    /// <response code="400">The status value isn't one of the allowed values.</response>
    [HttpGet("Status/{status}")]
    [ProducesResponseType(typeof(List<TicketDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<List<TicketDto>>> GetByStatus(string status)
    {
        var tickets = await _ticketService.GetTicketsByStatusAsync(status);
        return Ok(tickets);
    }
}
