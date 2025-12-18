using AirportTool.Application.DTOs.FlightSchedule;
using AirportTool.Application.Exceptions;
using AirportTool.Application.Interfaces;
using AirportTool.Application.Interfaces.ServiceInterfaces;
using AirportTool.Domain.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace AirportTool.Infrastructure.Services
{
    public class FlightScheduleService : IFlightScheduleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FlightScheduleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<FlightScheduleReadDto?> GetScheduleByIdAsync(int scheduleId, CancellationToken cancellationToken = default)
        {
            var schedule = await _unitOfWork.FlightSchedules.GetByIdAsync(scheduleId);
            if (schedule == null)
                throw new NotFoundException(nameof(FlightSchedule), scheduleId);

            return _mapper.Map<FlightScheduleReadDto>(schedule);
        }

        public async Task<IEnumerable<FlightScheduleReadDto>> GetUpcomingSchedulesAsync(int days = 7, CancellationToken cancellationToken = default)
        {
            var schedules = await _unitOfWork.FlightSchedules.GetUpcomingSchedulesAsync(days, cancellationToken);
            return _mapper.Map<IEnumerable<FlightScheduleReadDto>>(schedules);
        }

        public async Task<FlightScheduleReadDto> CreateScheduleAsync(FlightScheduleCreateDto dto, CancellationToken cancellationToken = default)
        {
            var entity = _mapper.Map<FlightSchedule>(dto);

            await _unitOfWork.FlightSchedules.AddAsync(entity);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<FlightScheduleReadDto>(entity);
        }

        public async Task<ImportResultDto> ImportSchedulesFromFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new BadRequestException("Invalid file");
            }

            if (!file.FileName.EndsWith(".json"))
            {
                throw new BadRequestException("Only JSON files are allowed");
            }

            List<FlightScheduleImportRowDto>? rows;

            using (var stream = file.OpenReadStream())
            {
                rows = await JsonSerializer.DeserializeAsync<List<FlightScheduleImportRowDto>>(
                    stream,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }

            if (rows == null || rows.Count == 0)
            {
                throw new BadRequestException("File is empty or invalid JSON");
            }

            var result = new ImportResultDto
            {
                Total = rows.Count
            };

            int rowIndex = 0;

            foreach (var row in rows)
            {
                rowIndex++;

                try
                {
                    var originAirport = await _unitOfWork.Airports.GetByIataCodeAsync(row.OriginIata);
                    var destinationAirport = await _unitOfWork.Airports.GetByIataCodeAsync(row.DestinationIata);

                    if (originAirport == null || destinationAirport == null)
                    {
                        throw new Exception("Invalid airport IATA");
                    }

                    var flights = await _unitOfWork.Flights.GetFlightsByRouteAsync(originAirport.Id, destinationAirport.Id, null);

                    var flight = flights.FirstOrDefault(f => f.FlightNumber == row.FlightNumber);

                    if (flight == null)
                    {
                        throw new Exception("Flight not found");
                    }

                    var existingSchedule = await _unitOfWork.FlightSchedules.GetByFlightAndDepartureAsync(flight.Id, row.ScheduledDepartureUtc);

                    if (existingSchedule == null)
                    {
                        var schedule = new FlightSchedule
                        {
                            FlightId = flight.Id,
                            ScheduledDepartureUtc = row.ScheduledDepartureUtc,
                            ScheduledArrivalUtc = row.ScheduledArrivalUtc,
                            FlightStatusId = 1
                        };

                        await _unitOfWork.FlightSchedules.AddAsync(schedule);
                        result.Created++;
                    }
                    else
                    {
                        existingSchedule.ScheduledArrivalUtc = row.ScheduledArrivalUtc;
                        result.Updated++;
                    }
                }
                catch (Exception ex)
                {
                    result.Errors.Add(new ImportErrorDto
                    {
                        Row = rowIndex,
                        Message = ex.Message
                    });
                }
            }

            await _unitOfWork.CompleteAsync();

            return result;
        }

    }
}
