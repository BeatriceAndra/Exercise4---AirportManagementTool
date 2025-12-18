using AirportTool.Application.Interfaces.Repositories;
using AirportTool.Domain.Entities;

namespace AirportTool.Application.Interfaces.Repository
{
    public interface IBookingRepository : IRepository<Booking>
    {
        Task<Booking?> GetBookingByConfirmationCodeAsync(string confirmationCode, CancellationToken cancellationToken = default);

        Task<IEnumerable<Booking>> GetBookingsForFlightScheduleAsync(int flightScheduleId, CancellationToken cancellationToken = default);
    }
}
