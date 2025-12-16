
namespace AirportTool.Application.DTOs.Ticket
{
    public class TicketCreateDto
    {
        public int FlightScheduleId { get; set; }
        public string FareClass { get; set; } = null!;
        public decimal BasePrice { get; set; }
        public decimal Taxes { get; set; }
        public string Currency { get; set; } = "EUR";
        public bool IsRefundable { get; set; } = true;
        public string SeatNumber { get; set; } = null!;
        public string PassengerFullName { get; set; } = null!;
        public string PassengerEmail { get; set; } = null!;
    }
}
