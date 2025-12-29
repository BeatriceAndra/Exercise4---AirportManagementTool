using AirportTool.Application.DTOs.FlightSchedule;
using Microsoft.AspNetCore.Http;

public interface IFlightScheduleImportParser
{
    Task<IReadOnlyList<FlightScheduleImportRowDto>> ParseAsync(IFormFile file);
}