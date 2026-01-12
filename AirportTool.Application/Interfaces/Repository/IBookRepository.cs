using AirportTool.Application.Interfaces.Repository;
using AirportTool.Domain.Entities;

namespace AirportTool.Application.Interfaces.Repository
{
    public interface IBookingRepository : IRepository<Booking>
    {
        Task<Booking?> GetBookingByConfirmationCodeAsync(string confirmationCode);

        Task<IEnumerable<Booking>> GetBookingsForFlightScheduleAsync(int flightScheduleId);
    }
}
