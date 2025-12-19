using AirportManagement.WebApi.Models;
using AirportTool.Application.Interfaces.Repositories;
using AirportTool.Application.Interfaces.Repository;
using AirportTool.Domain.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Airport = AirportTool.Domain.Entities.Airport;

namespace AirportTool.Infrastructure.Repositories
{
    public class AirportRepository : Repository<Airport>, IAirportRepository
    {
        private readonly AirportManagementContext _context;
        private readonly IMapper _mapper;

        public AirportRepository(AirportManagementContext context, IMapper mapper)
            : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Airport?> GetByIataCodeAsync(string iataCode, CancellationToken cancellationToken = default)
        {
            var airportDb = await _context.Airports.FirstOrDefaultAsync(a => a.IATACode == iataCode, cancellationToken);

            if (airportDb == null)
                return null;

            return _mapper.Map<Airport>(airportDb);
        }
    }
}
