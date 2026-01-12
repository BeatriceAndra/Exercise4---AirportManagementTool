using AirportTool.Application.DTOs.FlightSchedule;
using AirportTool.Application.Exceptions;
using AirportTool.Application.Interfaces;
using AirportTool.Infrastructure.Services;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;

namespace AirportTool.Tests.Services
{
    public class FlightScheduleServiceTests
    {
        private readonly FlightScheduleService _service;
        private readonly Mock<IUnitOfWork> _mockUoW = new();
        private readonly Mock<IMapper> _mockMapper = new();
        private readonly Mock<IFlightScheduleImportParser> _mockParser = new();
        private readonly Mock<ILogger<FlightScheduleService>> _mockLogger = new();

        public FlightScheduleServiceTests()
        {
            _service = new FlightScheduleService(_mockUoW.Object, _mockMapper.Object, _mockParser.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetScheduleByIdAsync_ScheduleExists_ReturnsDto()
        {
            // Arrange
            var schedule = new Domain.Entities.FlightSchedule { Id = 1 };
            _mockUoW.Setup(u => u.FlightSchedules.GetByIdAsync(1)).ReturnsAsync(schedule);
            _mockMapper.Setup(m => m.Map<FlightScheduleReadDto>(schedule)).Returns(new FlightScheduleReadDto());

            // Act
            var result = await _service.GetScheduleByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            _mockUoW.Verify(u => u.FlightSchedules.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetScheduleByIdAsync_ScheduleNotFound_ThrowsNotFoundException()
        {
            // Arrange
            _mockUoW.Setup(u => u.FlightSchedules.GetByIdAsync(1)).ReturnsAsync((Domain.Entities.FlightSchedule)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _service.GetScheduleByIdAsync(1));
        }

        [Fact]
        public async Task GetUpcomingSchedulesAsync_ReturnsMappedDtos()
        {
            // Arrange
            var schedules = new List<Domain.Entities.FlightSchedule>
            {
                new Domain.Entities.FlightSchedule { Id = 1 },
                new Domain.Entities.FlightSchedule { Id = 2 }
            };
            _mockUoW.Setup(u => u.FlightSchedules.GetUpcomingSchedulesAsync(7)).ReturnsAsync(schedules);
            _mockMapper.Setup(m => m.Map<IEnumerable<FlightScheduleReadDto>>(schedules)).Returns(new List<FlightScheduleReadDto> { new FlightScheduleReadDto(), new FlightScheduleReadDto() });

            // Act
            var result = await _service.GetUpcomingSchedulesAsync();

            // Assert
            Assert.Equal(2, result.Count());
            _mockUoW.Verify(u => u.FlightSchedules.GetUpcomingSchedulesAsync(7), Times.Once);
        }

        [Fact]
        public async Task CreateScheduleAsync_GateExists_WithOverlap_ThrowsGateOverlapException()
        {
            // Arrange
            var dto = new FlightScheduleCreateDto
            {
                FlightId = 1,
                ScheduledDepartureUtc = DateTime.UtcNow,
                ScheduledArrivalUtc = DateTime.UtcNow.AddHours(2),
                GateCode = "A1"
            };
            var gate = new Domain.Entities.Gate { Id = 10 };
            var schedule = new Domain.Entities.FlightSchedule { Id = 1 };

            _mockMapper.Setup(m => m.Map<Domain.Entities.FlightSchedule>(dto)).Returns(schedule);
            _mockUoW.Setup(u => u.Gates.GetByCodeAsync("A1")).ReturnsAsync(gate);
            _mockUoW.Setup(u => u.FlightSchedules.CheckGateOverlapAsync(gate.Id, dto.ScheduledDepartureUtc, dto.ScheduledArrivalUtc, null)).ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsAsync<GateOverlapException>(() => _service.CreateScheduleAsync(dto));
        }

        [Fact]
        public async Task ImportSchedulesFromFileAsync_AllValidRows_ReturnsResult()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.FileName).Returns("schedules.json");

            var row = new FlightScheduleImportRowDto
            {
                OriginIata = "AAA",
                DestinationIata = "BBB",
                FlightNumber = "FL123",
                ScheduledDepartureUtc = DateTime.UtcNow,
                ScheduledArrivalUtc = DateTime.UtcNow.AddHours(2)
            };

            var parsedRows = new List<FlightScheduleImportRowDto> { row };
            _mockParser.Setup(p => p.ParseAsync(fileMock.Object)).ReturnsAsync(parsedRows);

            var origin = new Domain.Entities.Airport { Id = 1 };
            var destination = new Domain.Entities.Airport { Id = 2 };
            var flight = new Domain.Entities.Flight { Id = 10, FlightNumber = "FL123" };

            _mockUoW.Setup(u => u.Airports.GetByIataCodeAsync("AAA")).ReturnsAsync(origin);
            _mockUoW.Setup(u => u.Airports.GetByIataCodeAsync("BBB")).ReturnsAsync(destination);
            _mockUoW.Setup(u => u.Flights.GetFlightsByRouteAsync(origin.Id, destination.Id, null)).ReturnsAsync(new List<Domain.Entities.Flight> { flight });
            _mockUoW.Setup(u => u.FlightSchedules.GetByFlightAndDepartureAsync(flight.Id, row.ScheduledDepartureUtc)).ReturnsAsync((Domain.Entities.FlightSchedule)null);

            // Act
            var result = await _service.ImportSchedulesFromFileAsync(fileMock.Object);

            // Assert
            Assert.Equal(1, result.Total);
            Assert.Equal(1, result.Created);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public async Task ImportSchedulesFromFileAsync_InvalidRow_AddsError()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.FileName).Returns("schedules.json");

            var row = new FlightScheduleImportRowDto
            {
                OriginIata = "AAA",
                DestinationIata = "XXX",
                FlightNumber = "FL123",
                ScheduledDepartureUtc = DateTime.UtcNow,
                ScheduledArrivalUtc = DateTime.UtcNow.AddHours(2)
            };

            var parsedRows = new List<FlightScheduleImportRowDto> { row };
            _mockParser.Setup(p => p.ParseAsync(fileMock.Object)).ReturnsAsync(parsedRows);

            _mockUoW.Setup(u => u.Airports.GetByIataCodeAsync("AAA")).ReturnsAsync(new Domain.Entities.Airport { Id = 1 });
            _mockUoW.Setup(u => u.Airports.GetByIataCodeAsync("XXX")).ReturnsAsync((Domain.Entities.Airport)null);

            // Act
            var result = await _service.ImportSchedulesFromFileAsync(fileMock.Object);

            // Assert
            Assert.Equal(1, result.Total);
            Assert.Equal(0, result.Created);
            Assert.Single(result.Errors);
            Assert.Contains("Invalid airport IATA code", result.Errors.First().Message);
        }
    }
}
