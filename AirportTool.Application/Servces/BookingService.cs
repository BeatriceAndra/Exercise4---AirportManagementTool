using AirportTool.Application.DTOs.Booking;
using AirportTool.Application.Exceptions;
using AirportTool.Application.Interfaces;
using AirportTool.Application.Interfaces.Repositories;
using AirportTool.Application.Interfaces.ServiceInterfaces;
using AirportTool.Domain.Entities;
using AirportTool.Domain.Enums;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace AirportTool.Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<BookingService> _logger;

        public BookingService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<BookingService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;

        }

        public async Task<BookingReadDto> CreateBookingAsync(BookingCreateDto dto, int userId)
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
            _logger.LogInformation("Creating booking for user {UserId}, ticket {TicketId}, quantity {Quantity}", userId, dto.TicketId, dto.Quantity);
            return _mapper.Map<BookingReadDto>(booking);

        }

        public async Task<BookingReadDto> GetByConfirmationCodeAsync(string confirmationCode)
        {
            var booking = await _unitOfWork.Bookings.GetBookingByConfirmationCodeAsync(confirmationCode);

            if (booking == null)
            {
                throw new NotFoundException(nameof(Booking), confirmationCode);
            }

            var tickets = await _unitOfWork.Tickets.GetTicketsByBookingAsync(booking.Id);
            var totalAmount = tickets.Sum(t => t.TotalPrice);

            return _mapper.Map<BookingReadDto>(booking);

        }

        private string GenerateConfirmationCode()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();

        }

        public async Task CancelBookingAsync(string confirmationCode)
        {
            var booking = await _unitOfWork.Bookings.GetBookingByConfirmationCodeAsync(confirmationCode);
            if (booking == null)
            {
                throw new NotFoundException(nameof(Booking), confirmationCode);
            }

            booking.Cancel();
            await _unitOfWork.Bookings.UpdateAsync(booking);
            await _unitOfWork.CompleteAsync();
            _logger.LogWarning("Cancelling booking with confirmation code {ConfirmationCode}", confirmationCode);

        }

    }
}
