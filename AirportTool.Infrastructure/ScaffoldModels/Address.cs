namespace AirportTool.Infrastructure;

public partial class Address
{
    public int Id { get; set; }

    public string Country { get; set; } = null!;

    public string City { get; set; } = null!;

    public string Street { get; set; } = null!;

    public virtual ICollection<Airport> Airports { get; set; } = new List<Airport>();
}
