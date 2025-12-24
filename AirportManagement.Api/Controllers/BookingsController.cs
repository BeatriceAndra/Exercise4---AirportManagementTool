using AirportTool.Application.DTOs.Booking;
using AirportTool.Application.Exceptions;
using AirportTool.Application.Interfaces.ServiceInterfaces;
using AirportTool.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirportTool.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        // POST: api/bookings
        [HttpPost]
        //[Authorize(Roles = Roles.Client)]
        public async Task<ActionResult<BookingReadDto>> CreateBooking([FromBody] BookingCreateDto dto)
        {
            int userId = 1;

            var booking = await _bookingService.CreateBookingAsync(dto, userId);

            return CreatedAtAction(nameof(GetBooking), new { code = booking.ConfirmationCode }, booking);
        }

        // GET: api/bookings/{code}
        [HttpGet("{code}")]
        //[Authorize(Roles = Roles.Staff)]
        public async Task<ActionResult<BookingReadDto>> GetBooking(string code)
        {
            try
            {
                var booking = await _bookingService.GetByConfirmationCodeAsync(code);
                return Ok(booking);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        // DELETE: api/bookings/{code}
        [HttpDelete("{code}")]
        //[Authorize(Roles = Roles.Staff)]
        public async Task<IActionResult> CancelBooking(string code)
        {
            try
            {
                await _bookingService.CancelBookingAsync(code);
                return NoContent();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
    }
}
