using AirportManagement.WebApi.Models;
using AirportTool.Application.Interfaces;
using AirportTool.Application.Interfaces.Repositories;
using AirportTool.Application.Interfaces.Repository;

namespace AirportTool.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AirportManagementContext _context;

        public IFlightRepository Flights { get; }
        public IFlightScheduleRepository FlightSchedules { get; }
        public ITicketRepository Tickets { get; }
        public IBookingRepository Bookings { get; }
        public IAirportRepository Airports { get; private set; }


        public UnitOfWork(
            AirportManagementContext context,
            IFlightRepository flights,
            IFlightScheduleRepository flightSchedules,
            ITicketRepository tickets,
            IBookingRepository bookings,
            IAirportRepository airports)
        {
            _context = context;
            Flights = flights;
            FlightSchedules = flightSchedules;
            Tickets = tickets;
            Bookings = bookings;
            Airports = airports;
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
