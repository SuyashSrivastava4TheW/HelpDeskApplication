using AutoMapper;
using HelpDesk.Api.DTOs;
using HelpDesk.Api.Exceptions;
using HelpDesk.Api.Models;
using HelpDesk.Api.Repositories;
using Microsoft.Extensions.Logging;

namespace HelpDesk.Api.Services;

public class TicketService : ITicketService
{
    private readonly ITicketRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<TicketService> _logger;

    public TicketService(ITicketRepository repository, IMapper mapper, ILogger<TicketService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<List<TicketDto>> GetAllTicketsAsync()
    {
        _logger.LogInformation("Fetching all tickets.");
        var tickets = await _repository.GetAllTicketsAsync();
        return _mapper.Map<List<TicketDto>>(tickets);
    }

    public async Task<TicketDto> GetTicketByIdAsync(int id)
    {
        var ticket = await _repository.GetTicketByIdAsync(id);
        if (ticket is null)
        {
            _logger.LogWarning("Ticket {TicketId} was requested but does not exist.", id);
            throw new NotFoundException($"Ticket with id {id} was not found.");
        }

        return _mapper.Map<TicketDto>(ticket);
    }

    public async Task<TicketDto> CreateTicketAsync(TicketCreateDto dto)
    {
        var ticket = _mapper.Map<Ticket>(dto);
        ticket.Status = TicketConstants.DefaultStatus; // hardcoded to "Open" on create
        ticket.CreatedDate = DateTime.UtcNow;

        var newId = await _repository.CreateTicketAsync(ticket);
        _logger.LogInformation("Created ticket {TicketId} raised by {RaisedBy}.", newId, ticket.RaisedBy);

        ticket.Id = newId;
        return _mapper.Map<TicketDto>(ticket);
    }

    public async Task<TicketDto> UpdateTicketAsync(int id, TicketUpdateDto dto)
    {
        var existing = await _repository.GetTicketByIdAsync(id);
        if (existing is null)
        {
            _logger.LogWarning("Attempted to update non-existent ticket {TicketId}.", id);
            throw new NotFoundException($"Ticket with id {id} was not found.");
        }

        _mapper.Map(dto, existing);
        await _repository.UpdateTicketAsync(existing);
        _logger.LogInformation("Updated ticket {TicketId}.", id);

        return _mapper.Map<TicketDto>(existing);
    }

    public async Task DeleteTicketAsync(int id)
    {
        var existing = await _repository.GetTicketByIdAsync(id);
        if (existing is null)
        {
            _logger.LogWarning("Attempted to delete non-existent ticket {TicketId}.", id);
            throw new NotFoundException($"Ticket with id {id} was not found.");
        }

        await _repository.DeleteTicketAsync(id);
        _logger.LogInformation("Deleted ticket {TicketId}.", id);
    }

    public async Task<List<TicketDto>> GetTicketsByStatusAsync(string status)
    {
        if (!TicketConstants.IsValidStatus(status))
        {
            _logger.LogWarning("Ticket status filter requested with invalid status {Status}.", status);
            throw new BadRequestException(
                $"'{status}' is not a valid status. Valid values are: {string.Join(", ", TicketConstants.ValidStatuses)}.");
        }

        var tickets = await _repository.GetTicketsByStatusAsync(status);
        return _mapper.Map<List<TicketDto>>(tickets);
    }
}
