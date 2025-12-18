using AirportTool.Domain.Entities;

namespace AirportTool.Application.Interfaces.Repositories
{
    public interface IFlightScheduleRepository : IRepository<FlightSchedule>
    {
        Task<IEnumerable<FlightSchedule>> GetSchedulesByFlightAsync(int flightId, CancellationToken cancellationToken = default);
        Task<IEnumerable<FlightSchedule>> GetUpcomingSchedulesAsync(int days, CancellationToken cancellationToken = default);
        Task<bool> CheckGateOverlapAsync(int gateId, DateTime departure, DateTime arrival, int? ignoreScheduleId = null, CancellationToken cancellationToken = default);
        Task<FlightSchedule?> GetByFlightAndDepartureAsync(int flightId, DateTime scheduledDepartureUtc, CancellationToken cancellationToken = default);
    }
}
