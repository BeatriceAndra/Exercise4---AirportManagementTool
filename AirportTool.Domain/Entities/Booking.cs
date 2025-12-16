using AirportTool.Domain.Enums;

namespace AirportTool.Domain.Entities
{
    public class Booking
    {
        public int Id { get; set; }
        public int FlightScheduleId { get; set; }
        public int TicketId { get; set; }
        public string PassengerFullName { get; set; } = null!;
        public string PassengerEmail { get; set; } = null!;
        public int Quantity { get; set; }
        public BookingStatus Status { get; set; } = BookingStatus.Active;
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

        public void Cancel()
        {
            Status = BookingStatus.Cancelled;
        }
    }
}
