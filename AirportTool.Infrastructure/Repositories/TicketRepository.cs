using AirportTool.Application.Interfaces.Repositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AirportTool.Infrastructure.Repositories
{
    public class TicketRepository : Repository<Domain.Entities.Ticket>, ITicketRepository
    {
        private readonly AirportManagementContext _context;
        private readonly IMapper _mapper;

        public TicketRepository(AirportManagementContext context, IMapper mapper): base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Domain.Entities.Ticket>> GetByFlightScheduleAsync(int flightScheduleId)
        {
            var tickets = await _context.Tickets.Where(t => t.FlightScheduleId == flightScheduleId).ToListAsync();

            return _mapper.Map<IEnumerable<Domain.Entities.Ticket>>(tickets);
        }

        public async Task<int> GetSoldTicketsCountAsync(int flightScheduleId)
        {
            return await _context.Tickets.CountAsync(t => t.FlightScheduleId == flightScheduleId);
        }
        public async Task<IEnumerable<Domain.Entities.Ticket>> GetTicketsByFlightAsync(int flightId)
        {
            var tickets = await _context.Tickets.Where(t => t.FlightSchedule.FlightId == flightId).ToListAsync();

            return _mapper.Map<IEnumerable<Domain.Entities.Ticket>>(tickets);
        }
        public async Task<IEnumerable<Domain.Entities.Ticket>> GetTicketsByBookingAsync(int bookingId)
        {
            var tickets = await _context.Tickets.Where(t => t.BookingId == bookingId).ToListAsync();

            return _mapper.Map<IEnumerable<Domain.Entities.Ticket>>(tickets);
        }
    }
}
