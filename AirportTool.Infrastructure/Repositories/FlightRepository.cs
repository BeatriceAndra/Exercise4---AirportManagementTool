using AirportTool.Application.Interfaces.Repository;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AirportTool.Infrastructure.Repository
{
    public class FlightRepository : Repository<Domain.Entities.Flight>, IFlightRepository
    {
        private readonly AirportManagementContext _context;
        private readonly IMapper _mapper;

        public FlightRepository(AirportManagementContext context, IMapper mapper) : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Domain.Entities.Flight?> GetFlightWithSchedulesAsync(int flightId)
        {
            var flightEf = await _context.Flight.Include(f => f.Schedules).FirstOrDefaultAsync(f => f.Id == flightId);

            return flightEf == null ? null : _mapper.Map<Domain.Entities.Flight>(flightEf);
        }

        public async Task<IEnumerable<Domain.Entities.Flight>> GetFlightsByRouteAsync(int originAirportId, int destinationAirportId, DateTime? date = null)
        {
            var query = _context.Flight.Include(f => f.Schedules).Where(f => f.OriginAirportId == originAirportId && f.DestinationAirportId == destinationAirportId);

            if (date.HasValue)
            {
                query = query.Where(f => f.Schedules.Any(fs => fs.ScheduledDepartureUtc.Date == date.Value.Date));
            }

            var flightsEf = await query.ToListAsync();

            return _mapper.Map<IEnumerable<Domain.Entities.Flight>>(flightsEf);
        }
    }
}
