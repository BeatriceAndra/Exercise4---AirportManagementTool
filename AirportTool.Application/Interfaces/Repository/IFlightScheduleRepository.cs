using AirportTool.Domain.Entities;

namespace AirportTool.Application.Interfaces.Repositories
{
    public interface IFlightScheduleRepository : IRepository<FlightSchedule>
    {
        Task<IEnumerable<FlightSchedule>> GetSchedulesByFlightAsync(int flightId);
        Task<IEnumerable<FlightSchedule>> GetUpcomingSchedulesAsync(int days);
        Task<bool> CheckGateOverlapAsync(int gateId, DateTime departure, DateTime arrival, int? ignoreScheduleId = null);
        Task<FlightSchedule?> GetByFlightAndDepartureAsync(int flightId, DateTime scheduledDepartureUtct);
    }
}
