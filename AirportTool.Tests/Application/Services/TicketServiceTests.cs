using AirportTool.Application.DTOs.Ticket;
using AirportTool.Application.Exceptions;
using AirportTool.Application.Interfaces;
using AirportTool.Application.Services;
using AirportTool.Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;

namespace AirportTool.Tests.ApplicationServices
{
    public class TicketServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUoW;
        private readonly Mock<ILogger<TicketService>> _mockLogger;
        private readonly TicketService _ticketService;

        public TicketServiceTests()
        {
            _mockUoW = new Mock<IUnitOfWork>();
            _mockLogger = new Mock<ILogger<TicketService>>();
            _ticketService = new TicketService(_mockUoW.Object, null, _mockLogger.Object);
        }

        [Theory]
        [InlineData(10, new int[] { 3, 2 }, 5)]
        [InlineData(5, new int[] { 2, 3 }, 0)]
        [InlineData(10, new int[] { }, 10)]
        public async Task CalculateSeatsAvailableAsync_VariousBookings_ReturnsExpected(int capacity, int[] bookedQuantities, int expectedSeats)
        {
            // Arrange
            var aircraftId = 1;
            var aircraft = new Aircraft { Id = aircraftId, SeatCapacity = capacity };

            var bookings = bookedQuantities.Select((q, i) => new Booking { Id = i + 1, Quantity = q }).ToList();

            _mockUoW.Setup(u => u.Aircrafts.GetByIdAsync(aircraftId)).ReturnsAsync(aircraft);
            _mockUoW.Setup(u => u.Bookings.GetAllAsync()).ReturnsAsync(bookings);

            // Act
            var seatsAvailable = await _ticketService.CalculateSeatsAvailableAsync(aircraftId);

            // Assert
            Assert.Equal(expectedSeats, seatsAvailable);
        }

        [Fact]
        public async Task CalculateSeatsAvailableAsync_AircraftNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var aircraftId = 1;
            _mockUoW.Setup(u => u.Aircrafts.GetByIdAsync(aircraftId)).ReturnsAsync((Aircraft)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() =>_ticketService.CalculateSeatsAvailableAsync(aircraftId)
            );
        }

        [Theory]
        [InlineData(-100, 20)]
        [InlineData(100, -10)]
        public async Task CreateTicketAsync_NegativeValues_ThrowArgumentException(decimal basePrice, decimal taxes)
        {
            // Arrange
            var dto = new TicketCreateDto
            {
                BasePrice = basePrice,
                Taxes = taxes
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>_ticketService.CreateTicketAsync(dto));

            _mockUoW.Verify(u => u.Tickets.AddAsync(It.IsAny<Ticket>()), Times.Never);
            _mockUoW.Verify(u => u.CompleteAsync(), Times.Never);
        }

        [Fact]
        public async Task DeleteTicketAsync_TicketExists_DeletesTicket()
        {
            var ticket = new Ticket { Id = 1 };
            _mockUoW.Setup(u => u.Tickets.GetByIdAsync(1)).ReturnsAsync(ticket);

            await _ticketService.DeleteTicketAsync(1);

            _mockUoW.Verify(u => u.Tickets.DeleteAsync(1), Times.Once);
            _mockUoW.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteTicketAsync_TicketDoesNotExist_ThrowsNotFoundException()
        {
            _mockUoW.Setup(u => u.Tickets.GetByIdAsync(1)).ReturnsAsync((Domain.Entities.Ticket)null);

            await Assert.ThrowsAsync<NotFoundException>(() =>_ticketService.DeleteTicketAsync(1));
        }
    }
}
