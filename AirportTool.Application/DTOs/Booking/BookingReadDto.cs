
namespace AirportTool.Application.DTOs.Booking
{
    public class BookingReadDto
    {
        public string ConfirmationCode { get; set; } = null!;
        public string Status { get; set; } = null!;
        public decimal TotalAmount { get; set; }
    }
}
