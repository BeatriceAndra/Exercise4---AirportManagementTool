using AirportTool.Domain.Entities;

namespace AirportTool.Application.Interfaces.Repositories
{
    public interface IFlightRepository : IRepository<Flight>
    {
        Task<Flight?> GetFlightWithSchedulesAsync(int flightId);
        Task<IEnumerable<Flight>> GetFlightsByRouteAsync(int originAirportId, int destinationAirportId, DateTime? date = null);
    }
}
