namespace AirportTool.Domain.Entities
{
    public class Flight
    {
        public int Id { get; set; }
        public int AirlineId { get; set; }
        public string FlightNumber { get; set; } = null!;
        public int OriginAirportId { get; set; }
        public int DestinationAirportId { get; set; }
        public int? DefaultAircraftId { get; set; }
        public bool IsActive { get; set; } = true;

        public void ValidateRoute()
        {
            if (OriginAirportId == DestinationAirportId)
                throw new InvalidOperationException("Origin and destination cannot be the same.");
        }
    }
}
