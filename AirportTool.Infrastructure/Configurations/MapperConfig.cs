using AutoMapper;

namespace AirportTool.Infrastructure.Configurations
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<AirportTool.Infrastructure.Flight, Domain.Entities.Flight>().ForMember(d => d.Schedules, o => o.MapFrom(s => s.FlightSchedules)).ReverseMap();

            CreateMap<AirportTool.Infrastructure.FlightSchedule, Domain.Entities.FlightSchedule>().ForMember(d => d.Tickets, o => o.MapFrom(s => s.Tickets)).ReverseMap();

            CreateMap<AirportTool.Infrastructure.Ticket, Domain.Entities.Ticket>().ReverseMap();

            CreateMap<AirportTool.Infrastructure.Booking, Domain.Entities.Booking>().ForMember(d => d.Tickets, o => o.MapFrom(s => s.Tickets)).ReverseMap();

        }
    }
}
