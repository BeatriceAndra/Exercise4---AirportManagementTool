using AirportManagement.WebApi.Models;
using AirportTool.Application.Interfaces.Repositories;
using AirportTool.Domain.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Ticket = AirportTool.Domain.Entities.Ticket;

namespace AirportTool.Infrastructure.Repositories
{
    public class TicketRepository : Repository<Ticket>, ITicketRepository
    {
        private readonly AirportManagementContext _context;
        private readonly IMapper _mapper;

        public TicketRepository(AirportManagementContext context, IMapper mapper): base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Ticket>> GetByFlightScheduleAsync(int flightScheduleId, CancellationToken cancellationToken = default)
        {
            var tickets = await _context.Tickets
                .Where(t => t.FlightScheduleId == flightScheduleId)
                .ToListAsync(cancellationToken);

            return _mapper.Map<IEnumerable<Ticket>>(tickets);
        }

        public async Task<int> GetSoldTicketsCountAsync(int flightScheduleId, CancellationToken cancellationToken = default)
        {
            return await _context.Tickets
                .CountAsync(t => t.FlightScheduleId == flightScheduleId, cancellationToken);
        }
        public async Task<IEnumerable<Ticket>> GetTicketsByBookingAsync(int bookingId, CancellationToken cancellationToken = default)
        {
            var tickets =  await _context.Tickets
                .Where(t => t.BookingId == bookingId)
                .ToListAsync(cancellationToken);

            return _mapper.Map<IEnumerable<Ticket>>(tickets);
        }
    }
}
