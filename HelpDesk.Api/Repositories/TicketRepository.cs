using HelpDesk.Api.Data;
using HelpDesk.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Api.Repositories;

/// <summary>
/// Concrete EF Core implementation of ITicketRepository. All database logic
/// (querying, tracking, saving) lives here and nowhere else — the service
/// layer above never touches AppDbContext.
/// </summary>
public class TicketRepository : ITicketRepository
{
    private readonly AppDbContext _context;

    public TicketRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Ticket>> GetAllTicketsAsync()
    {
        return await _context.Tickets
            .AsNoTracking()
            .OrderByDescending(t => t.CreatedDate)
            .ToListAsync();
    }

    public async Task<Ticket?> GetTicketByIdAsync(int id)
    {
        return await _context.Tickets
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<int> CreateTicketAsync(Ticket ticket)
    {
        _context.Tickets.Add(ticket);
        await _context.SaveChangesAsync();
        return ticket.Id;
    }

    public async Task UpdateTicketAsync(Ticket ticket)
    {
        _context.Tickets.Update(ticket);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteTicketAsync(int id)
    {
        var ticket = await _context.Tickets.FindAsync(id);
        if (ticket is null)
        {
            // Idempotent delete: caller (service layer) is responsible for
            // deciding whether "not found" should surface as a 404. The
            // repository itself doesn't throw for a missing row.
            return;
        }

        _context.Tickets.Remove(ticket);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Ticket>> GetTicketsByStatusAsync(string status)
    {
        return await _context.Tickets
            .AsNoTracking()
            .Where(t => t.Status == status)
            .OrderByDescending(t => t.CreatedDate)
            .ToListAsync();
    }
}
