using AirportManagement.WebApi.Models;
using AirportTool.Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using FlightSchedule = AirportTool.Domain.Entities.FlightSchedule;

namespace AirportTool.Infrastructure.Repositories
{
    public class FlightScheduleRepository : Repository<FlightSchedule>, IFlightScheduleRepository
    {
        public FlightScheduleRepository(AirportManagementContext context) : base(context)
        {
        }

        public async Task<IEnumerable<FlightSchedule>> GetSchedulesByFlightAsync(int flightId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(fs => fs.FlightId == flightId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<FlightSchedule>> GetUpcomingSchedulesAsync(int days, CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;
            var endDate = now.AddDays(days);

            return await _dbSet
                .Where(fs => fs.ScheduledDepartureUtc >= now && fs.ScheduledDepartureUtc <= endDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> CheckGateOverlapAsync(int gateId, DateTime departure, DateTime arrival, int? ignoreScheduleId = null, CancellationToken cancellationToken = default)
        {
            var query = _dbSet.AsQueryable()
                .Where(fs => fs.GateId == gateId);

            if (ignoreScheduleId.HasValue)
                query = query.Where(fs => fs.Id != ignoreScheduleId.Value);

            return await query.AnyAsync(fs =>
                fs.ScheduledDepartureUtc < arrival &&
                fs.ScheduledArrivalUtc > departure,
                cancellationToken);
        }
    }
}
