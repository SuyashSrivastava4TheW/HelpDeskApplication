using HelpDesk.Api.Models;

namespace HelpDesk.Api.Repositories;

/// <summary>
/// Abstraction over all Ticket persistence operations. The service layer
/// depends only on this interface, never on EF Core or DbContext directly,
/// so the data-access technology can be swapped/mocked without touching
/// business logic (Dependency Inversion Principle).
/// </summary>
public interface ITicketRepository
{
    Task<List<Ticket>> GetAllTicketsAsync();

    Task<Ticket?> GetTicketByIdAsync(int id);

    Task<int> CreateTicketAsync(Ticket ticket);

    Task UpdateTicketAsync(Ticket ticket);

    Task DeleteTicketAsync(int id);

    Task<List<Ticket>> GetTicketsByStatusAsync(string status);
}
