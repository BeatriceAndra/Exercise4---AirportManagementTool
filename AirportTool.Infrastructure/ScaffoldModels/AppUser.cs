using System;
using System.Collections.Generic;

namespace AirportManagement.WebApi.Models;

public partial class AppUser
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
