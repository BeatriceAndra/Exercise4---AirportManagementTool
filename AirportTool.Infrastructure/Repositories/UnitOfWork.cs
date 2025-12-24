using AirportTool.Application.Interfaces;
using AirportTool.Application.Interfaces.Repository;

namespace AirportTool.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AirportManagementContext _context;

        public IFlightRepository Flights { get; }
        public IFlightScheduleRepository FlightSchedules { get; }
        public ITicketRepository Tickets { get; }
        public IBookingRepository Bookings { get; }
        public IAirportRepository Airports { get; }
        public IGateRepository Gates { get; }
        public IAircraftRepository Aircrafts { get; }


        public UnitOfWork(
            AirportManagementContext context,
            IFlightRepository flights,
            IFlightScheduleRepository flightSchedules,
            ITicketRepository tickets,
            IBookingRepository bookings,
            IAirportRepository airports,
            IGateRepository gates,
            IAircraftRepository aircrafts)
        {
            _context = context;
            Flights = flights;
            FlightSchedules = flightSchedules;
            Tickets = tickets;
            Bookings = bookings;
            Airports = airports;
            Gates = gates;
            Aircrafts = aircrafts;
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
