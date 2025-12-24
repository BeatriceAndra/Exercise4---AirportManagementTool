using AirportTool.Application.Interfaces.Repository;
using AutoMapper;

namespace AirportTool.Infrastructure.Repository
{
    public class AircraftRepository : Repository<Domain.Entities.Aircraft>, IAircraftRepository
    {
        private static IMapper mapper;
        public AircraftRepository(AirportManagementContext context) : base(context, mapper)
        {
        }
    }
}
