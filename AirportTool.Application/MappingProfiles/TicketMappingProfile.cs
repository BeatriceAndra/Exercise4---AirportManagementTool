using AirportTool.Application.DTOs.Ticket;
using AirportTool.Domain.Entities;
using AutoMapper;

namespace AirportTool.Application.MappingProfiles;

public class TicketMappingProfile : Profile
{
    public TicketMappingProfile()
    {
        // Domain -> Read
        CreateMap<Ticket, TicketReadDto>()
            .ForMember(d => d.SeatsAvailable, o => o.Ignore());

        // Create -> Domain
        CreateMap<TicketCreateDto, Ticket>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.TotalPrice, o => o.Ignore())
            .ForMember(d => d.SeatNumber, o => o.Ignore());
    }
}
