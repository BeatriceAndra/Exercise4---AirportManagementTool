using AirportTool.Application.Interfaces.Repository;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AirportTool.Infrastructure.Repositories
{
    public class AirportRepository : Repository<Domain.Entities.Airport>, IAirportRepository
    {
        private readonly AirportManagementContext _context;
        private readonly IMapper _mapper;

        public AirportRepository(AirportManagementContext context, IMapper mapper)
            : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Domain.Entities.Airport?> GetByIataCodeAsync(string iataCode)
        {
            var airportDb = await _context.Airports.FirstOrDefaultAsync(a => a.IATACode == iataCode);

            if (airportDb == null)
                return null;

            return _mapper.Map<Domain.Entities.Airport>(airportDb);
        }
    }
}
