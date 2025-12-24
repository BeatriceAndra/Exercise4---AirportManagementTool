using AirportTool.Infrastructure.Scaffold.ScaffoldModels;
using AutoMapper;

namespace AirportTool.Infrastructure.Configurations
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Flight, Domain.Entities.Flight>().ForMember(d => d.Schedules, o => o.MapFrom(s => s.FlightSchedules)).ReverseMap();

            CreateMap<FlightSchedule, Domain.Entities.FlightSchedule>().ForMember(d => d.Tickets, o => o.MapFrom(s => s.Tickets)).ReverseMap();

            CreateMap<Ticket, Domain.Entities.Ticket>().ReverseMap();

            CreateMap<Booking, Domain.Entities.Booking>().ForMember(d => d.Tickets, o => o.MapFrom(s => s.Tickets)).ReverseMap();

        }
    }
}
