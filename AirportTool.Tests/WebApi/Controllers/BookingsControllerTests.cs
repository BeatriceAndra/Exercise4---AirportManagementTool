using AirportTool.Application.DTOs.Booking;
using AirportTool.Application.Exceptions;
using AirportTool.Application.Interfaces.ServiceInterfaces;
using AirportTool.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AirportTool.Tests.Controllers
{
    public class BookingsControllerTests
    {
        private readonly Mock<IBookingService> _mockService = new();
        private readonly BookingsController _controller;

        public BookingsControllerTests()
        {
            _controller = new BookingsController(_mockService.Object);
        }

        [Fact]
        public async Task CreateBooking_ValidDto_ReturnsCreatedAtAction()
        {
            // Arrange
            var dto = new BookingCreateDto { TicketId = 1, Quantity = 2 };
            var bookingDto = new BookingReadDto { ConfirmationCode = "ABC12345" };

            _mockService.Setup(s => s.CreateBookingAsync(dto, It.IsAny<int>())).ReturnsAsync(bookingDto);

            // Act
            var result = await _controller.CreateBooking(dto);

            // Assert
            var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(201, actionResult.StatusCode);
            Assert.Equal(bookingDto, actionResult.Value);
            _mockService.Verify(s => s.CreateBookingAsync(dto, It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task GetBooking_ExistingCode_ReturnsOk()
        {
            // Arrange
            var code = "ABC12345";
            var bookingDto = new BookingReadDto { ConfirmationCode = code };

            _mockService.Setup(s => s.GetByConfirmationCodeAsync(code)).ReturnsAsync(bookingDto);

            // Act
            var result = await _controller.GetBooking(code);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, actionResult.StatusCode);
            Assert.Equal(bookingDto, actionResult.Value);
        }

        [Fact]
        public async Task GetBooking_NonExistingCode_ReturnsNotFound()
        {
            // Arrange
            var code = "NOTEXIST";
            _mockService.Setup(s => s.GetByConfirmationCodeAsync(code)).ThrowsAsync(new NotFoundException(nameof(BookingReadDto), code));

            // Act
            var result = await _controller.GetBooking(code);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CancelBooking_ExistingCode_ReturnsNoContent()
        {
            // Arrange
            var code = "ABC12345";

            _mockService.Setup(s => s.CancelBookingAsync(code)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CancelBooking(code);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockService.Verify(s => s.CancelBookingAsync(code), Times.Once);
        }

        [Fact]
        public async Task CancelBooking_NonExistingCode_ReturnsNotFound()
        {
            // Arrange
            var code = "NOTEXIST";

            _mockService.Setup(s => s.CancelBookingAsync(code)).ThrowsAsync(new NotFoundException(nameof(BookingReadDto), code));

            // Act
            var result = await _controller.CancelBooking(code);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
