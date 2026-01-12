using AirportManagement.WebApi.Controllers;
using AirportTool.Application.DTOs.Ticket;
using AirportTool.Application.Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AirportTool.Tests.Controllers
{
    public class TicketsControllerTests
    {
        private readonly Mock<ITicketService> _mockService = new();
        private readonly TicketsController _controller;

        public TicketsControllerTests()
        {
            _controller = new TicketsController(_mockService.Object);
        }

        [Fact]
        public async Task GetTicketsByFlight_ReturnsOkWithTickets()
        {
            // Arrange
            int flightId = 1;
            var tickets = new List<TicketReadDto>
            {
                new TicketReadDto { Id = 1, FlightScheduleId = flightId },
                new TicketReadDto { Id = 2, FlightScheduleId = flightId }
            };

            _mockService.Setup(s => s.GetTicketsByFlightScheduleIdAsync(flightId)).ReturnsAsync(tickets);

            // Act
            var result = await _controller.GetTicketsByFlight(flightId);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(tickets, actionResult.Value);
        }

        [Fact]
        public async Task CreateTicket_ValidDto_ReturnsCreatedAtAction()
        {
            // Arrange
            var dto = new TicketCreateDto { FlightScheduleId = 1, BasePrice = 100, Taxes = 20 };
            var createdDto = new TicketReadDto { Id = 1, FlightScheduleId = dto.FlightScheduleId };

            _mockService.Setup(s => s.CreateTicketAsync(dto)).ReturnsAsync(createdDto);

            // Act
            var result = await _controller.CreateTicket(dto);

            // Assert
            var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(createdDto, actionResult.Value);
            Assert.Equal(nameof(_controller.GetTicketsByFlight), actionResult.ActionName);
        }

        [Fact]
        public async Task DeleteTicket_ValidId_ReturnsNoContent()
        {
            // Arrange
            int ticketId = 1;

            _mockService.Setup(s => s.DeleteTicketAsync(ticketId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteTicket(ticketId);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockService.Verify(s => s.DeleteTicketAsync(ticketId), Times.Once);
        }
    }
}
