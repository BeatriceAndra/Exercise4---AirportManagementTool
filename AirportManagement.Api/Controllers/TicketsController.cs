using AirportTool.Application.DTOs.Ticket;
using AirportTool.Application.Interfaces.ServiceInterfaces;
using AirportTool.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AirportManagement.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketsController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        // GET /api/tickets/by-flight/{flightId}
        [HttpGet("by-flight/{flightId}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<TicketReadDto>>> GetTicketsByFlight(int flightId)
        {
            var tickets = await _ticketService.GetTicketsByFlightAsync(flightId);
            return Ok(tickets);
        }

        // POST /api/tickets
        [HttpPost]
        [Authorize(Roles = Roles.Staff)]
        public async Task<ActionResult<TicketReadDto>> CreateTicket([FromBody] TicketCreateDto dto)
        {
            var ticket = await _ticketService.CreateTicketAsync(dto);
            return CreatedAtAction(nameof(GetTicketsByFlight), new { flightId = ticket.FlightScheduleId }, ticket);
        }

        // DELETE /api/tickets/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Staff)]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            await _ticketService.DeleteTicketAsync(id);
            return NoContent();
        }
    }
}
