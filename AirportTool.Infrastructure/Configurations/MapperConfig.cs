using AutoMapper;

namespace AirportTool.Application.MappingProfiles
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<AirportManagement.WebApi.Models.Flight, Domain.Entities.Flight>()
                .ForMember(d => d.Schedules, o => o.MapFrom(s => s.FlightSchedules));

            CreateMap<AirportManagement.WebApi.Models.FlightSchedule, Domain.Entities.FlightSchedule>()
                .ForMember(d => d.Tickets, o => o.MapFrom(s => s.Tickets));

            CreateMap<AirportManagement.WebApi.Models.Ticket, Domain.Entities.Ticket>();

            CreateMap<AirportManagement.WebApi.Models.Booking, Domain.Entities.Booking>()
                .ForMember(d => d.Tickets, o => o.MapFrom(s => s.Tickets));

        }
    }
}
