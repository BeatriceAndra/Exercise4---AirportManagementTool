using AirportTool.Domain.Entities;

namespace AirportTool.Application.Interfaces.Repositories
{
    public interface IGateRepository : IRepository<Gate>
    {
        Task<Gate?> GetByCodeAsync(string gateCode);
        Task<IEnumerable<Gate>> GetAllAsync();
    }
}
