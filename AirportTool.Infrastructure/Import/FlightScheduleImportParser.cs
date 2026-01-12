using AirportTool.Application.DTOs.FlightSchedule;
using AirportTool.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

public class FlightScheduleImportParser : IFlightScheduleImportParser
{
    public async Task<IReadOnlyList<FlightScheduleImportRowDto>> ParseAsync(IFormFile file)
    {
        using var stream = file.OpenReadStream();

        var rows = await JsonSerializer.DeserializeAsync<List<FlightScheduleImportRowDto>>(stream,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }
            );

        if (rows == null || rows.Count == 0)
            throw new BadRequestException("Import file is empty or invalid.");

        return rows;
    }
}
