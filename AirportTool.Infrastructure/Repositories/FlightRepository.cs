using AirportManagement.WebApi.Models;
using AirportTool.Application.Interfaces.Repositories;
using AirportTool.Domain.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Flight = AirportTool.Domain.Entities.Flight;

namespace AirportTool.Infrastructure.Repositories
{
    public class FlightRepository : Repository<Flight>, IFlightRepository
    {
        private readonly AirportManagementContext _context;
        private readonly IMapper _mapper;

        public FlightRepository(AirportManagementContext context, IMapper mapper) : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Flight?> GetFlightWithSchedulesAsync(int flightId, CancellationToken cancellationToken = default)
        {
            var flightEf = await _context.Flights.Include(f => f.FlightSchedules).FirstOrDefaultAsync(f => f.Id == flightId, cancellationToken);

            return flightEf == null ? null : _mapper.Map<Flight>(flightEf);
        }

        public async Task<IEnumerable<Flight>> GetFlightsByRouteAsync(int originAirportId, int destinationAirportId, DateTime? date = null, CancellationToken cancellationToken = default)
        {
            var query = _context.Flights.Include(f => f.FlightSchedules).Where(f => f.OriginAirportId == originAirportId && f.DestinationAirportId == destinationAirportId);

            if (date.HasValue)
            {
                query = query.Where(f => f.FlightSchedules.Any(fs => fs.ScheduledDepartureUtc.Date == date.Value.Date));
            }

            var flightsEf = await query.ToListAsync(cancellationToken);

            return _mapper.Map<IEnumerable<Flight>>(flightsEf);
        }
    }
}
