using AirportTool.Domain.Entities;

namespace AirportTool.Application.Interfaces.Repository
{
    public interface IAirportRepository
    {
        Task<Airport?> GetByIataCodeAsync(string iataCode);
    }
}
