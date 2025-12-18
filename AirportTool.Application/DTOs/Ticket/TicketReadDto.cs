
namespace AirportTool.Application.DTOs.Ticket
{
    public class TicketReadDto
    {
        public int Id { get; set; }
        public string FareClass { get; set; } = null!;
        public decimal TotalPrice { get; set; }
        public string Currency { get; set; } = "EUR";
        public bool IsRefundable { get; set; }
        public int SeatsAvailable { get; set; }
    }
}
