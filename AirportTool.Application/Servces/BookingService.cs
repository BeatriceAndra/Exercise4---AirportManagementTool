using AirportTool.Application.DTOs.Booking;
using AirportTool.Application.Exceptions;
using AirportTool.Application.Interfaces;
using AirportTool.Application.Interfaces.Repositories;
using AirportTool.Application.Interfaces.ServiceInterfaces;
using AirportTool.Domain.Entities;
using AirportTool.Domain.Enums;
using AutoMapper;

namespace AirportTool.Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BookingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BookingReadDto> CreateBookingAsync(BookingCreateDto dto, int userId, CancellationToken cancellationToken = default)
        {
            var ticket = await _unitOfWork.Tickets.GetByIdAsync(dto.TicketId);
            if (ticket == null)
            {
                throw new NotFoundException(nameof(Ticket), dto.TicketId);
            }

            var totalAmount = ticket.TotalPrice * dto.Quantity;

            var booking = new Booking
            {
                UserId = userId,
                BookingStatusId = 1,
                ConfirmationCode = GenerateConfirmationCode(),
                Quantity = dto.Quantity,
                CreatedUtc = DateTime.UtcNow,
            };

            await _unitOfWork.Bookings.AddAsync(booking);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<BookingReadDto>(booking);
        }

        public async Task<BookingReadDto> GetByConfirmationCodeAsync(string confirmationCode, CancellationToken cancellationToken = default)
        {
            var booking = await _unitOfWork.Bookings.GetBookingByConfirmationCodeAsync(confirmationCode, cancellationToken);

            if (booking == null)
            {
                throw new NotFoundException(nameof(Booking), confirmationCode);
            }

            var tickets = await _unitOfWork.Tickets.GetTicketsByBookingAsync(booking.Id, cancellationToken);
            var totalAmount = tickets.Sum(t => t.TotalPrice);

            return _mapper.Map<BookingReadDto>(booking);
        }

        private string GenerateConfirmationCode()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
        }

        public async Task CancelBookingAsync(string confirmationCode, CancellationToken cancellationToken = default)
        {
            var booking = await _unitOfWork.Bookings.GetBookingByConfirmationCodeAsync(confirmationCode, cancellationToken);
            if (booking == null)
            {
                throw new NotFoundException(nameof(Booking), confirmationCode);
            }

            booking.Cancel();

            await _unitOfWork.Bookings.UpdateAsync(booking);
            await _unitOfWork.CompleteAsync();
        }

    }
}
