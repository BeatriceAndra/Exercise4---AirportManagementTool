using AirportTool.Application.DTOs.FlightSchedule;
using AirportTool.Domain.Entities;
using AutoMapper;

namespace AirportTool.Application.MappingProfiles;

public class FlightScheduleMappingProfile : Profile
{
    public FlightScheduleMappingProfile()
    {
        // FlightSchedule -> FlightScheduleReadDto
        CreateMap<FlightSchedule, FlightScheduleReadDto>()
            .ForMember(d => d.GateCode, o => o.Ignore())
            .ForMember(d => d.AssignedAircraftTail, o => o.Ignore())
            .ForMember(d => d.FlightNumber, o => o.Ignore())
            .ForMember(d => d.AirlineIata, o => o.Ignore())
            .ForMember(d => d.OriginIata, o => o.Ignore())
            .ForMember(d => d.DestinationIata, o => o.Ignore());

        // FlightScheduleCreateDto -> FlightSchedule
        CreateMap<FlightScheduleCreateDto, FlightSchedule>()
            .ForMember(d => d.GateId, o => o.Ignore())
            .ForMember(d => d.AssignedAircraftId, o => o.Ignore());
    }
}
