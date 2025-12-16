
namespace AirportTool.Application.DTOs.Ticket
{
    public class TicketReadDto
    {
        public int Id { get; set; }
        public int FlightScheduleId { get; set; }
        public string FareClass { get; set; } = null!;
        public decimal BasePrice { get; set; }
        public decimal Taxes { get; set; }
        public decimal TotalPrice { get; set; }
        public string Currency { get; set; } = "EUR";
        public bool IsRefundable { get; set; }
        public string SeatNumber { get; set; } = null!;
        public string PassengerFullName { get; set; } = null!;
        public string PassengerEmail { get; set; } = null!;
    }
}
