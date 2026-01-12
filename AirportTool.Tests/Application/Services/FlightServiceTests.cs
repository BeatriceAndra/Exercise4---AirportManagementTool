using AirportTool.Application.DTOs.Flight;
using AirportTool.Application.Exceptions;
using AirportTool.Application.Interfaces;
using AirportTool.Application.Services;
using AirportTool.Domain.Entities;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;

namespace AirportTool.Tests.Services
{
    public class FlightServiceTests
    {
        private readonly FlightService _flightService;
        private readonly Mock<IUnitOfWork> _mockUoW = new();
        private readonly Mock<IMapper> _mockMapper = new();
        private readonly Mock<ILogger<FlightService>> _mockLogger = new();

        public FlightServiceTests()
        {
            _flightService = new FlightService(_mockUoW.Object, _mockMapper.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetFlightWithSchedulesAsync_FlightExists_ReturnsFlightReadDto()
        {
            // Arrange
            var flight = new Flight { Id = 1 };
            _mockUoW.Setup(u => u.Flights.GetFlightWithSchedulesAsync(1)).ReturnsAsync(flight);
            _mockMapper.Setup(m => m.Map<FlightReadDto>(flight)).Returns(new FlightReadDto());

            // Act
            var result = await _flightService.GetFlightWithSchedulesAsync(1);

            // Assert
            Assert.NotNull(result);
            _mockUoW.Verify(u => u.Flights.GetFlightWithSchedulesAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetFlightWithSchedulesAsync_FlightNotFound_ThrowsNotFoundException()
        {
            // Arrange
            _mockUoW.Setup(u => u.Flights.GetFlightWithSchedulesAsync(1)).ReturnsAsync((Flight)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _flightService.GetFlightWithSchedulesAsync(1)
            );
        }

        [Fact]
        public async Task GetFlightsByRouteAsync_FlightsExist_ReturnsFlightReadDtos()
        {
            // Arrange
            var flights = new List<Flight>
            {
                new Flight { Id = 1 },
                new Flight { Id = 2 }
            };
            _mockUoW.Setup(u => u.Flights.GetFlightsByRouteAsync(1, 2, null)).ReturnsAsync(flights);
            _mockMapper.Setup(m => m.Map<IEnumerable<FlightReadDto>>(flights)).Returns(new List<FlightReadDto> { new FlightReadDto(), new FlightReadDto() });

            // Act
            var result = await _flightService.GetFlightsByRouteAsync(1, 2);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _mockUoW.Verify(u => u.Flights.GetFlightsByRouteAsync(1, 2, null), Times.Once);
        }

        [Fact]
        public async Task GetFlightsByRouteAsync_NoFlights_ThrowsNotFoundException()
        {
            // Arrange
            _mockUoW.Setup(u => u.Flights.GetFlightsByRouteAsync(1, 2, null)).ReturnsAsync(new List<Flight>());

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() =>_flightService.GetFlightsByRouteAsync(1, 2)
            );
        }

        [Fact]
        public async Task UpdateFlightAsync_FlightExists_UpdatesFlight()
        {
            // Arrange
            var dto = new FlightUpdateDto { FlightNumber = "CD456" };
            var flight = new Flight { Id = 1, FlightNumber = "AB123" };

            _mockUoW.Setup(u => u.Flights.GetByIdAsync(1)).ReturnsAsync(flight);
            _mockUoW.Setup(u => u.Flights.UpdateAsync(flight)).Returns(Task.CompletedTask);
            _mockMapper.Setup(m => m.Map(dto, flight));

            // Act
            await _flightService.UpdateFlightAsync(1, dto);

            // Assert
            _mockUoW.Verify(u => u.Flights.UpdateAsync(flight), Times.Once);
            _mockUoW.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateFlightAsync_FlightNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var dto = new FlightUpdateDto { FlightNumber = "CD456" };
            _mockUoW.Setup(u => u.Flights.GetByIdAsync(1)).ReturnsAsync((Flight)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() =>_flightService.UpdateFlightAsync(1, dto)
            );
        }

        [Fact]
        public async Task DeleteFlightAsync_FlightExists_DeletesFlight()
        {
            // Arrange
            var flight = new Flight { Id = 1 };
            _mockUoW.Setup(u => u.Flights.GetByIdAsync(1)).ReturnsAsync(flight);
            _mockUoW.Setup(u => u.Flights.DeleteAsync(1)).Returns(Task.CompletedTask);

            // Act
            await _flightService.DeleteFlightAsync(1);

            // Assert
            _mockUoW.Verify(u => u.Flights.DeleteAsync(1), Times.Once);
            _mockUoW.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteFlightAsync_FlightNotFound_ThrowsNotFoundException()
        {
            // Arrange
            _mockUoW.Setup(u => u.Flights.GetByIdAsync(1)).ReturnsAsync((Flight)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() =>_flightService.DeleteFlightAsync(1)
            );
        }
    }
}
