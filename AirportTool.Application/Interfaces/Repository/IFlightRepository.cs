using AirportTool.Domain.Entities;

namespace AirportTool.Application.Interfaces.Repositories
{
    public interface IFlightRepository : IRepository<Flight>
    {
        Task<Flight?> GetFlightWithSchedulesAsync(int flightId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Flight>> GetFlightsByRouteAsync(int originAirportId, int destinationAirportId, DateTime? date = null, CancellationToken cancellationToken = default);
    }
}
