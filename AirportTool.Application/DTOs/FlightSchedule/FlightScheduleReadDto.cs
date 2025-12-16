
namespace AirportTool.Application.DTOs.FlightSchedule
{
    public class FlightScheduleReadDto
    {
        public int Id { get; set; }
        public int FlightId { get; set; }
        public DateTime ScheduledDepartureUtc { get; set; }
        public DateTime ScheduledArrivalUtc { get; set; }
        public string? GateCode { get; set; }
        public string? AssignedAircraftTail { get; set; }
        public string Status { get; set; } = null!;
    }
}
