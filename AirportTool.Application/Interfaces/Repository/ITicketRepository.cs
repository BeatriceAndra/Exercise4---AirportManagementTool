using AirportTool.Domain.Entities;

namespace AirportTool.Application.Interfaces.Repositories
{
    public interface ITicketRepository : IRepository<Ticket>
    {
        Task<IEnumerable<Ticket>> GetByFlightScheduleAsync(int flightScheduleId);
        Task<int> GetSoldTicketsCountAsync(int flightScheduleId);
        Task<IEnumerable<Ticket>> GetTicketsByFlightAsync(int flightId);
        Task<IEnumerable<Ticket>> GetTicketsByBookingAsync(int bookingId);
    }
}
