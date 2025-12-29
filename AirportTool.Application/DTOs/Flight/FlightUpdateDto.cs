namespace AirportTool.Application.DTOs.Flight
{
    public class FlightUpdateDto
    {
        public string FlightNumber { get; set; } = null!;
        public string OriginIata { get; set; } = null!;
        public string DestinationIata { get; set; } = null!;
        public string? DefaultAircraftTail { get; set; }
        public bool IsActive { get; set; }
    }
}
