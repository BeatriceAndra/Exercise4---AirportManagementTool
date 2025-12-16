using AirportTool.Domain.Enums;

namespace AirportTool.Domain.Entities
{
    public class FlightSchedule
    {
        public int Id { get; set; }
        public int FlightId { get; set; }
        public DateTime ScheduledDepartureUtc { get; set; }
        public DateTime ScheduledArrivalUtc { get; set; }
        public int? GateId { get; set; }
        public int? AssignedAircraftId { get; set; }
        public FlightStatus Status { get; set; } = FlightStatus.Planned;

        public void ValidateTimes()
        {
            if (ScheduledArrivalUtc <= ScheduledDepartureUtc)
                throw new InvalidOperationException("ScheduledArrivalUtc must be after ScheduledDepartureUtc.");
        }
    }
}
