namespace AirportTool.Domain.Entities
{
    public class Airport
    {
        public int Id { get; set; }
        public string IATACode { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string TimeZone { get; set; } = null!;
        public int AddressId { get; set; }
        public IReadOnlyCollection<Gate> Gates { get; set; } = new List<Gate>();
    }
}
