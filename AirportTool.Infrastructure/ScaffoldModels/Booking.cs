using System;
using System.Collections.Generic;

namespace AirportManagement.WebApi.Models;

public partial class Booking
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int BookingStatusId { get; set; }

    public string ConfirmationCode { get; set; } = null!;

    public int Quantity { get; set; }

    public DateTime CreatedUtc { get; set; }

    public virtual BookingStatus BookingStatus { get; set; } = null!;

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public virtual AppUser User { get; set; } = null!;
}
