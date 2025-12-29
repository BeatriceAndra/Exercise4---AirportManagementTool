using AirportTool.Domain.Enums;

namespace AirportTool.Domain.Entities
{
    public class Booking
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public int BookingStatusId { get; set; }
        public string ConfirmationCode { get; set; } = null!;
        public int Quantity { get; set; }
        public DateTime CreatedUtc { get; set; }
        public IReadOnlyCollection<Ticket> Tickets { get; set; } = new List<Ticket>();
        public void Cancel()
        {
            BookingStatusId = (int)Enums.BookingStatus.Cancelled;
        }
    }
}
