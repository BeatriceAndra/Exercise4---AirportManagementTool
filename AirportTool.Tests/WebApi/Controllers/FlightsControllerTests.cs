using AirportManagement.WebApi.Controllers;
using AirportTool.Application.DTOs.Flight;
using AirportTool.Application.Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AirportTool.Tests.Controllers
{
    public class FlightsControllerTests
    {
        private readonly Mock<IFlightService> _mockService = new();
        private readonly FlightsController _controller;

        public FlightsControllerTests()
        {
            _controller = new FlightsController(_mockService.Object);
        }

        [Fact]
        public async Task GetFlights_ReturnsOkWithFlights()
        {
            // Arrange
            int originId = 1, destinationId = 2;
            var date = new DateTime(2025, 12, 1);
            var flights = new List<FlightReadDto>
            {
                new FlightReadDto { Id = 1 },
                new FlightReadDto { Id = 2 }
            };

            _mockService.Setup(s => s.GetFlightsByRouteAsync(originId, destinationId, date)).ReturnsAsync(flights);

            // Act
            var result = await _controller.GetFlights(originId, destinationId, date);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(flights, actionResult.Value);
        }

        [Fact]
        public async Task CreateFlight_OriginEqualsDestination_ReturnsBadRequest()
        {
            // Arrange
            var dto = new FlightCreateDto { OriginIata = "OTP", DestinationIata = "OTP" };

            // Act
            var result = await _controller.CreateFlight(dto);

            // Assert
            var actionResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Origin and destination airports cannot be the same.", actionResult.Value);
        }

        [Fact]
        public async Task CreateFlight_ValidDto_ReturnsCreatedAtAction()
        {
            // Arrange
            var dto = new FlightCreateDto { OriginIata = "OTP", DestinationIata = "LHR" };
            var createdDto = new FlightReadDto { Id = 1 };

            _mockService.Setup(s => s.CreateFlightAsync(dto)).ReturnsAsync(createdDto);

            // Act
            var result = await _controller.CreateFlight(dto);

            // Assert
            var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(createdDto, actionResult.Value);
            Assert.Equal(nameof(_controller.GetFlights), actionResult.ActionName);
        }

        [Fact]
        public async Task UpdateFlight_ValidId_ReturnsOk()
        {
            // Arrange
            int id = 1;
            var dto = new FlightUpdateDto { FlightNumber = "F123" };

            _mockService.Setup(s => s.UpdateFlightAsync(id, dto)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateFlight(id, dto);

            // Assert
            Assert.IsType<OkResult>(result);
            _mockService.Verify(s => s.UpdateFlightAsync(id, dto), Times.Once);
        }

        [Fact]
        public async Task DeleteFlight_ValidId_ReturnsNoContent()
        {
            // Arrange
            int id = 1;
            _mockService.Setup(s => s.DeleteFlightAsync(id)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteFlight(id);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockService.Verify(s => s.DeleteFlightAsync(id), Times.Once);
        }
    }
}
