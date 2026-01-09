using AirportTool.Application.DTOs.Booking;
using AirportTool.Application.Exceptions;
using AirportTool.Application.Interfaces;
using AirportTool.Application.Services;
using AirportTool.Domain.Entities;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;

namespace AirportTool.Tests.Services
{
    public class BookingServiceTests
    {
        private readonly BookingService _bookingService;
        private readonly Mock<IUnitOfWork> _mockUoW = new();
        private readonly Mock<IMapper> _mockMapper = new();
        private readonly Mock<ILogger<BookingService>> _mockLogger = new();

        public BookingServiceTests()
        {
            _bookingService = new BookingService(_mockUoW.Object, _mockMapper.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task CreateBookingAsync_TicketNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var dto = new BookingCreateDto { TicketId = 1, Quantity = 2 };
            _mockUoW.Setup(u => u.Tickets.GetByIdAsync(1)).ReturnsAsync((Ticket)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() =>_bookingService.CreateBookingAsync(dto, userId: 123)
            );

            _mockUoW.Verify(u => u.Bookings.AddAsync(It.IsAny<Domain.Entities.Booking>()), Times.Never);
            _mockUoW.Verify(u => u.CompleteAsync(), Times.Never);
        }

        [Fact]
        public async Task GetByConfirmationCodeAsync_BookingExists_ReturnsBookingReadDto()
        {
            // Arrange
            var booking = new Booking { Id = 1 };
            var tickets = new List<Ticket>
            {
                new Ticket { BasePrice = 80, Taxes = 20 },
                new Ticket { BasePrice = 150, Taxes = 50 }
            };

            _mockUoW.Setup(u => u.Bookings.GetBookingByConfirmationCodeAsync("ABC123")).ReturnsAsync(booking);
            _mockUoW.Setup(u => u.Tickets.GetTicketsByBookingAsync(1)).ReturnsAsync(tickets);
            _mockMapper.Setup(m => m.Map<BookingReadDto>(booking)).Returns(new BookingReadDto());

            // Act
            var result = await _bookingService.GetByConfirmationCodeAsync("ABC123");

            // Assert
            Assert.NotNull(result);
            _mockUoW.Verify(u => u.Bookings.GetBookingByConfirmationCodeAsync("ABC123"), Times.Once);
            _mockUoW.Verify(u => u.Tickets.GetTicketsByBookingAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetByConfirmationCodeAsync_BookingNotFound_ThrowsNotFoundException()
        {
            // Arrange
            _mockUoW.Setup(u => u.Bookings.GetBookingByConfirmationCodeAsync("ABC123")).ReturnsAsync((Domain.Entities.Booking)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() =>_bookingService.GetByConfirmationCodeAsync("ABC123")
            );
        }

        [Fact]
        public async Task CancelBookingAsync_BookingExists_CancelsBooking()
        {
            // Arrange
            var booking = new Domain.Entities.Booking { Id = 1 };
            _mockUoW.Setup(u => u.Bookings.GetBookingByConfirmationCodeAsync("ABC123")).ReturnsAsync(booking);
            _mockUoW.Setup(u => u.Bookings.UpdateAsync(booking)).Returns(Task.CompletedTask);

            // Act
            await _bookingService.CancelBookingAsync("ABC123");

            // Assert
            _mockUoW.Verify(u => u.Bookings.UpdateAsync(booking), Times.Once);
            _mockUoW.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task CancelBookingAsync_BookingNotFound_ThrowsNotFoundException()
        {
            // Arrange
            _mockUoW.Setup(u => u.Bookings.GetBookingByConfirmationCodeAsync("ABC123")).ReturnsAsync((Domain.Entities.Booking)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() =>_bookingService.CancelBookingAsync("ABC123")
            );
        }
    }
}