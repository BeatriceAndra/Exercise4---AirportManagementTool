using AirportTool.Domain.Entities;

namespace AirportTool.Application.Interfaces.Repositories
{
    public interface ITicketRepository : IRepository<Ticket>
    {
        Task<IEnumerable<Ticket>> GetByFlightScheduleAsync(int flightScheduleId, CancellationToken cancellationToken = default);
        Task<int> GetSoldTicketsCountAsync(int flightScheduleId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Ticket>> GetTicketsByBookingAsync(int bookingId, CancellationToken cancellationToken = default);
    }
}
