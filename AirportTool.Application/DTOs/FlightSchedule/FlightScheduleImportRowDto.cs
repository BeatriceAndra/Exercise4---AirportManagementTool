
namespace AirportTool.Application.DTOs.FlightSchedule
{
    public class FlightScheduleImportRowDto
    {
        public string FlightNumber { get; set; } = null!;
        public string AirlineIata { get; set; } = null!;
        public string OriginIata { get; set; } = null!;
        public string DestinationIata { get; set; } = null!;
        public DateTime ScheduledDepartureUtc { get; set; }
        public DateTime ScheduledArrivalUtc { get; set; }
        public string? GateCode { get; set; }
        public string? AssignedAircraftTail { get; set; }
    }
}
