using AirportTool.Application.DTOs.FlightSchedule;
using AirportTool.Application.Interfaces.ServiceInterfaces;
using AirportTool.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirportManagement.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchedulesController : ControllerBase
    {
        private readonly IFlightScheduleService _flightScheduleService;

        public SchedulesController(IFlightScheduleService flightScheduleService)
        {
            _flightScheduleService = flightScheduleService;
        }

        // GET /api/schedules/{id} -> schedule with gate, aircraft, status
        [HttpGet("{id}")]
        //[AllowAnonymous]
        public async Task<ActionResult<FlightScheduleReadDto>> GetById(int id)
        {
            var schedule = await _flightScheduleService.GetScheduleByIdAsync(id);
            if (schedule == null)
                return NotFound();

            return Ok(schedule);
        }

        // GET /api/schedules/stats/upcoming -> flights for next 7 days
        [HttpGet("stats/upcoming")]
        //[Authorize(Roles = Roles.Staff)]
        public async Task<ActionResult> GetUpcomingStats()
        {
            var stats = await _flightScheduleService.GetUpcomingSchedulesAsync();
            return Ok(stats);
        }

        // POST /api/schedules -> create one schedule (Planned)
        [HttpPost]
        //[Authorize(Roles = Roles.Staff)]
        public async Task<ActionResult<FlightScheduleReadDto>> Create([FromBody] FlightScheduleCreateDto dto)
        {
            var schedule = await _flightScheduleService.CreateScheduleAsync(dto);
            if (schedule == null)
                return NotFound();
            return CreatedAtAction(nameof(GetById), new { id = schedule.Id }, schedule);
        }

        // POST /api/schedules/import -> JSON file import
        [HttpPost("import")]
        [Consumes("multipart/form-data")]
        //[Authorize(Roles = Roles.Staff)]
        public async Task<IActionResult> Import([FromForm] UploadFileDto dto)
        {
            if (dto.File == null || dto.File.Length == 0)
                return BadRequest("No file provided.");

            var result = await _flightScheduleService.ImportSchedulesFromFileAsync(dto.File);

            if (result.Errors.Any())
                return StatusCode(207, result);

            if (result.Created + result.Updated == result.Total)
                return Created("", result);

            return BadRequest(result);
        }
    }
}

