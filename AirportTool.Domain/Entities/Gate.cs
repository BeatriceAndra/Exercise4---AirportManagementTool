namespace AirportTool.Domain.Entities
{
    public class Gate
    {
        public int Id { get; set; }
        public int AirportId { get; set; }
        public string Code { get; set; } = null!;
    }
}
