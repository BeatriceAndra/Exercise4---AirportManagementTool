using AirportTool.Domain.Entities;
using AirportTool.Application.Exceptions;
using AirportManagement.WebApi.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Booking = AirportTool.Domain.Entities.Booking;
using AirportTool.Application.Interfaces.Repository;

namespace AirportTool.Infrastructure.Repositories
{
    public class BookingRepository : Repository<Booking>, IBookingRepository
    {
        private readonly AirportManagementContext _context;
        private readonly IMapper _mapper;

        public BookingRepository(
            AirportManagementContext context,
            IMapper mapper)
            : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Booking?> GetBookingByConfirmationCodeAsync(
            string confirmationCode,
            CancellationToken cancellationToken = default)
        {
            var bookingDb = await _context.Bookings
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    b => b.ConfirmationCode == confirmationCode,
                    cancellationToken);

            if (bookingDb == null)
            {
                throw new NotFoundException(
                    nameof(Booking),
                    confirmationCode);
            }

            return _mapper.Map<Booking>(bookingDb);
        }

        public async Task<IEnumerable<Booking>> GetBookingsForFlightScheduleAsync(
            int flightScheduleId,
            CancellationToken cancellationToken = default)
        {
            var bookingsDb = await _context.Bookings
                .AsNoTracking()
                .Where(b =>
                    b.Tickets.Any(t =>
                        t.FlightScheduleId == flightScheduleId))
                .ToListAsync(cancellationToken);

            return _mapper.Map<IEnumerable<Booking>>(bookingsDb);
        }
    }
}
