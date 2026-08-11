using AutoMapper;
using HelpDesk.Api.DTOs;
using HelpDesk.Api.Models;

namespace HelpDesk.Api.Mapping;

public class TicketMappingProfile : Profile
{
    public TicketMappingProfile()
    {
        CreateMap<Ticket, TicketDto>();
        CreateMap<TicketCreateDto, Ticket>();
        CreateMap<TicketUpdateDto, Ticket>();
    }
}
