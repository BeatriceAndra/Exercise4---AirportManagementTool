using AirportTool.Application.DTOs.FlightSchedule;

namespace AirportTool.Application.Interfaces.ServiceInterfaces
{
    public interface IFlightScheduleService
    {
        Task<FlightScheduleReadDto?> GetScheduleByIdAsync(int scheduleId, CancellationToken cancellationToken = default);
        Task<IEnumerable<FlightScheduleReadDto>> GetUpcomingSchedulesAsync(int days = 7, CancellationToken cancellationToken = default);
        Task<FlightScheduleReadDto> CreateScheduleAsync(FlightScheduleCreateDto dto, CancellationToken cancellationToken = default);
        Task<IEnumerable<FlightScheduleImportRowDto>> ImportSchedulesAsync(IEnumerable<FlightScheduleImportRowDto> schedules, CancellationToken cancellationToken = default);
    }
}
