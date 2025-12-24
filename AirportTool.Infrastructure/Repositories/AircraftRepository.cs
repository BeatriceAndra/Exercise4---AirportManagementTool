using AirportTool.Application.Interfaces.Repository;

namespace AirportTool.Infrastructure.Repositories
{
    public class AircraftRepository : Repository<Domain.Entities.Aircraft>, IAircraftRepository
    {
        public AircraftRepository(AirportManagementContext context) : base(context)
        {
        }
    }
}
