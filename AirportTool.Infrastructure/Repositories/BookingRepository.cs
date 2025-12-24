using AirportTool.Application.Exceptions;
using AirportTool.Application.Interfaces.Repository;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AirportTool.Infrastructure.Repositories
{
    public class BookingRepository : Repository<Domain.Entities.Booking>, IBookingRepository
    {
        private readonly AirportManagementContext _context;
        private readonly IMapper _mapper;

        public BookingRepository(AirportManagementContext context, IMapper mapper) : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Domain.Entities.Booking?> GetBookingByConfirmationCodeAsync(string confirmationCode)
        {
            var bookingDb = await _context.Bookings.AsNoTracking().FirstOrDefaultAsync(b => b.ConfirmationCode == confirmationCode);

            if (bookingDb == null)
            {
                throw new NotFoundException(nameof(Booking), confirmationCode);
            }

            return _mapper.Map<Domain.Entities.Booking>(bookingDb);
        }

        public async Task<IEnumerable<Domain.Entities.Booking>> GetBookingsForFlightScheduleAsync(int flightScheduleId)
        {
            var bookingsDb = await _context.Bookings.AsNoTracking().Where(b => b.Tickets.Any(t => t.FlightScheduleId == flightScheduleId)).ToListAsync();

            return _mapper.Map<IEnumerable<Domain.Entities.Booking>>(bookingsDb);
        }
    }
}
