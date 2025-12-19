using AirportTool.Application.DTOs.Flight;
using AirportTool.Application.Interfaces.ServiceInterfaces;
using AirportTool.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AirportManagement.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Roles.Staff)]
    public class FlightsController : ControllerBase
    {
        private readonly IFlightService _flightService;

        public FlightsController(IFlightService flightService)
        {
            _flightService = flightService;
        }

        // GET /api/flights?origin=OTP&destination=LHR&date=2025-12-01
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FlightReadDto>>> GetFlights([FromQuery] int originId, [FromQuery] int destinationId, [FromQuery] DateTime? date)
        {
            var flights = await _flightService.GetFlightsByRouteAsync(originId, destinationId, date);
            return Ok(flights);
        }

        // POST /api/flights
        [HttpPost]
        public async Task<ActionResult<FlightReadDto>> CreateFlight([FromBody] FlightCreateDto dto)
        {
            if (dto.OriginIata == dto.DestinationIata)
            {
                return BadRequest("Origin and destination airports cannot be the same.");
            }

            var flight = await _flightService.CreateFlightAsync(dto);
            return CreatedAtAction(nameof(GetFlights), new { id = flight.Id }, flight);
        }

        // PUT /api/flights/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateFlight(int id, [FromBody] FlightUpdateDto dto)
        {
            await _flightService.UpdateFlightAsync(id, dto);
            return Ok();
        }

        // DELETE /api/flights/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteFlight(int id)
        {
            await _flightService.DeleteFlightAsync(id);
            return NoContent();
        }
    }
}
