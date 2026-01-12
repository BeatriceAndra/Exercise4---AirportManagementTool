namespace AirportTool.Application.DTOs.Flight
{
    public class FlightReadDto
    {
        public int Id { get; set; }
        public string AirlineIata { get; set; } = null!;
        public string FlightNumber { get; set; } = null!;
        public string OriginIata { get; set; } = null!;
        public string DestinationIata { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
