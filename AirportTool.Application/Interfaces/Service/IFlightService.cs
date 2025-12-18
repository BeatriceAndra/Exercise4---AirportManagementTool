using AirportTool.Application.DTOs.Flight;

namespace AirportTool.Application.Interfaces.ServiceInterfaces
{
    public interface IFlightService
    {
        Task<FlightReadDto> GetFlightWithSchedulesAsync(int flightId);
        Task<IEnumerable<FlightReadDto>> GetFlightsByRouteAsync(int originAirportId, int destinationAirportId, DateTime? date = null);
        Task<FlightReadDto> CreateFlightAsync(FlightCreateDto dto);
        Task UpdateFlightAsync(int id, FlightUpdateDto dto);
        Task DeleteFlightAsync(int id);

    }
}
