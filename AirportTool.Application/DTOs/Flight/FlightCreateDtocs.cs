
namespace AirportTool.Application.DTOs.Flight
{
    public class FlightCreateDtocs
    {
        public string AirlineIata { get; set; } = null!;
        public string FlightNumber { get; set; } = null!;
        public string OriginIata { get; set; } = null!;
        public string DestinationIata { get; set; } = null!;
        public string? DefaultAircraftTail { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
