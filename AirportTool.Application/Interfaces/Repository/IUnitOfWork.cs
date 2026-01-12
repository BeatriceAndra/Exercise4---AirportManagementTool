
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
        IGateRepository Gates { get; }
        IAircraftRepository Aircrafts { get; }


        Task<int> CompleteAsync();
    }
}
