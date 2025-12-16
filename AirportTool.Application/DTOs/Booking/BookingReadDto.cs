
namespace AirportTool.Application.DTOs.Booking
{
    public class BookingReadDto
    {
        public int Id { get; set; }
        public string ConfirmationCode { get; set; } = null!;
        public string PassengerFullName { get; set; } = null!;
        public string PassengerEmail { get; set; } = null!;
        public int Quantity { get; set; }
        public string Status { get; set; } = null!;
        public decimal TotalAmount { get; set; }
    }
}
