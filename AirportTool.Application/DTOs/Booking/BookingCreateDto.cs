
namespace AirportTool.Application.DTOs.Booking
{
    public class BookingCreateDto
    {
        public int FlightScheduleId { get; set; }
        public int TicketId { get; set; }
        public string PassengerFullName { get; set; } = null!;
        public string PassengerEmail { get; set; } = null!;
        public int Quantity { get; set; }
    }
}
