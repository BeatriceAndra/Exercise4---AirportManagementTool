using AirportTool.Application.Interfaces.Repository;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AirportTool.Infrastructure.Repository
{
    public class FlightScheduleRepository : Repository<Domain.Entities.FlightSchedule>, IFlightScheduleRepository
    {
        private readonly AirportManagementContext _context;
        private readonly IMapper _mapper;
        public FlightScheduleRepository(AirportManagementContext context, IMapper mapper) : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Domain.Entities.FlightSchedule>> GetSchedulesByFlightAsync(int flightId)
        {
            return await _dbSet.Where(fs => fs.FlightId == flightId).ToListAsync();
        }

        public async Task<IEnumerable<Domain.Entities.FlightSchedule>> GetUpcomingSchedulesAsync(int days)
        {
            var now = DateTime.UtcNow;
            var endDate = now.AddDays(days);

            return await _dbSet.Where(fs => fs.ScheduledDepartureUtc >= now && fs.ScheduledDepartureUtc <= endDate).ToListAsync();
        }

        public async Task<bool> CheckGateOverlapAsync(int gateId, DateTime departure, DateTime arrival, int? ignoreScheduleId = null)
        {
            var query = _dbSet.AsQueryable().Where(fs => fs.GateId == gateId);

            if (ignoreScheduleId.HasValue)
                query = query.Where(fs => fs.Id != ignoreScheduleId.Value);

            return await query.AnyAsync(fs => fs.ScheduledDepartureUtc < arrival && fs.ScheduledArrivalUtc > departure);
        }
        public async Task<Domain.Entities.FlightSchedule?> GetByFlightAndDepartureAsync(int flightId, DateTime scheduledDepartureUtc)
        {
            var entity = await _context.FlightSchedule.FirstOrDefaultAsync(fs =>fs.FlightId == flightId && fs.ScheduledDepartureUtc == scheduledDepartureUtc);

            if (entity == null)
                return null;

            return _mapper.Map<Domain.Entities.FlightSchedule>(entity);
        }

    }
}
