namespace AirportTool.Domain.Entities
{
    public class Ticket
    {
        public int Id { get; set; }
        public int FlightScheduleId { get; set; }
        public string FareClass { get; set; } = null!;
        public decimal BasePrice { get; set; }
        public decimal Taxes { get; set; }
        public decimal TotalPrice => BasePrice + Taxes;
        public string Currency { get; set; } = "EUR";
        public bool IsRefundable { get; set; } = true;
        public int SeatInventory { get; set; }
        public string PassengerFullName { get; set; } = null!;
        public string PassengerEmail { get; set; } = null!;
    }
}
