using HelpDesk.Api.DTOs;

namespace HelpDesk.Api.Services;

/// <summary>
/// Business logic for Tickets. The controller depends only on this
/// interface — never on ITicketRepository directly — keeping HTTP concerns
/// (status codes, model binding) fully separate from business rules.
/// </summary>
public interface ITicketService
{
    Task<List<TicketDto>> GetAllTicketsAsync();

    Task<TicketDto> GetTicketByIdAsync(int id);

    Task<TicketDto> CreateTicketAsync(TicketCreateDto dto);

    Task<TicketDto> UpdateTicketAsync(int id, TicketUpdateDto dto);

    Task DeleteTicketAsync(int id);

    Task<List<TicketDto>> GetTicketsByStatusAsync(string status);
}
