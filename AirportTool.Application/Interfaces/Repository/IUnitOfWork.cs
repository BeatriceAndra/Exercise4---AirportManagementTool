
using AirportTool.Application.Interfaces.Repositories;
using AirportTool.Application.Interfaces.Repository;

namespace AirportTool.Application.Interfaces
{
    public interface IUnitOfWork
    {
        IFlightRepository Flights { get; }
        IFlightScheduleRepository FlightSchedules { get; }
        ITicketRepository Tickets { get; }
        IBookingRepository Bookings { get; }
        IAirportRepository Airports { get; }

        Task<int> CompleteAsync();
    }
}
