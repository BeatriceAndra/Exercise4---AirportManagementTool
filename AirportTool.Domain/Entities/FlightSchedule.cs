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
        public int FlightStatusId { get; set; }
        public IReadOnlyCollection<Ticket> Tickets { get; set; }
           = new List<Ticket>();
    }
}
