using AirportTool.Application.DTOs.FlightSchedule;
using Microsoft.AspNetCore.Http;

namespace AirportTool.Application.Interfaces.ServiceInterfaces
{
    public interface IFlightScheduleService
    {
        Task<FlightScheduleReadDto?> GetScheduleByIdAsync(int scheduleId);
        Task<IEnumerable<FlightScheduleReadDto>> GetUpcomingSchedulesAsync(int days = 7);
        Task<FlightScheduleReadDto> CreateScheduleAsync(FlightScheduleCreateDto dto);
        Task<ImportResultDto> ImportSchedulesFromFileAsync(IFormFile file);
    }
}
