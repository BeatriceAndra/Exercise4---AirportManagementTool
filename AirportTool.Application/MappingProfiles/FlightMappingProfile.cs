using AirportTool.Application.DTOs.Flight;
using AirportTool.Domain.Entities;
using AutoMapper;

public class FlightMappingProfile : Profile
{
    public FlightMappingProfile()
    {
        // Flight -> FlightReadDto
        CreateMap<Flight, FlightReadDto>();

        // FlightCreateDto -> Flight
        CreateMap<FlightCreateDto, Flight>()
            .ForMember(d => d.AirlineId, o => o.Ignore())
            .ForMember(d => d.OriginAirportId, o => o.Ignore())
            .ForMember(d => d.DestinationAirportId, o => o.Ignore())
            .ForMember(d => d.DefaultAircraftId, o => o.Ignore());

        // FlightUpdateDto -> Flight
        CreateMap<FlightUpdateDto, Flight>()
            .ForMember(d => d.AirlineId, o => o.Ignore())
            .ForMember(d => d.OriginAirportId, o => o.Ignore())
            .ForMember(d => d.DestinationAirportId, o => o.Ignore())
            .ForMember(d => d.DefaultAircraftId, o => o.Ignore());
    }
}