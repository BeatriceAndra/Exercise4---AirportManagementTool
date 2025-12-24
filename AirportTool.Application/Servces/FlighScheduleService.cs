using AirportTool.Application.DTOs.FlightSchedule;
using AirportTool.Application.Exceptions;
using AirportTool.Application.Interfaces;
using AirportTool.Application.Interfaces.ServiceInterfaces;
using AirportTool.Domain.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AirportTool.Infrastructure.Services
{
    public class FlightScheduleService : IFlightScheduleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFlightScheduleImportParser _importParser;
        private readonly ILogger<FlightScheduleService> _logger;

        public FlightScheduleService(IUnitOfWork unitOfWork, IMapper mapper, IFlightScheduleImportParser importParser, ILogger<FlightScheduleService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _importParser = importParser;
            _logger = logger;

        }

        public async Task<FlightScheduleReadDto?> GetScheduleByIdAsync(int scheduleId)
        {
            var schedule = await _unitOfWork.FlightSchedules.GetByIdAsync(scheduleId);
            if (schedule == null)
                throw new NotFoundException(nameof(FlightSchedule), scheduleId);

            return _mapper.Map<FlightScheduleReadDto>(schedule);

        }

        public async Task<IEnumerable<FlightScheduleReadDto>> GetUpcomingSchedulesAsync(int days = 7)
        {
            var schedules = await _unitOfWork.FlightSchedules.GetUpcomingSchedulesAsync(days);
            return _mapper.Map<IEnumerable<FlightScheduleReadDto>>(schedules);

        }

        public async Task<FlightScheduleReadDto> CreateScheduleAsync(FlightScheduleCreateDto dto)
        {
            var schedule = _mapper.Map<FlightSchedule>(dto);
            var gate = dto.GateCode == null ? null : await _unitOfWork.Gates.GetByCodeAsync(dto.GateCode);

            if (gate != null)
            {
                await EnsureGateNoOverlapAsync(gate.Id, dto.ScheduledDepartureUtc, dto.ScheduledArrivalUtc);
            }

            await _unitOfWork.FlightSchedules.AddAsync(schedule);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Flight schedule created. ScheduleId={ScheduleId}, FlightId={FlightId}, Departure={Departure}", schedule.Id, schedule.FlightId, schedule.ScheduledDepartureUtc);

            return _mapper.Map<FlightScheduleReadDto>(schedule);

        }

        public async Task<ImportResultDto> ImportSchedulesFromFileAsync(IFormFile file)
        {
            var rows = await _importParser.ParseAsync(file);

            var result = new ImportResultDto
            {
                Total = rows.Count
            };

            var rowIndex = 0;
            _logger.LogInformation("Schedule import started. FileName={FileName}",file.FileName);

            foreach (var row in rows)
            {
                rowIndex++;

                try
                {
                    var originAirport = await _unitOfWork.Airports.GetByIataCodeAsync(row.OriginIata);

                    var destinationAirport = await _unitOfWork.Airports.GetByIataCodeAsync(row.DestinationIata);

                    if (originAirport == null || destinationAirport == null)
                        throw new Exception("Invalid airport IATA code");

                    var flights = await _unitOfWork.Flights.GetFlightsByRouteAsync(originAirport.Id, destinationAirport.Id, null);

                    var flight = flights.FirstOrDefault(f => f.FlightNumber == row.FlightNumber);

                    if (flight == null)
                        throw new Exception("Flight not found");

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
            _logger.LogInformation("Schedule import finished. Total={Total}, Created={Created}, Updated={Updated}, Errors={Errors}", result.Total, result.Created, result.Updated, result.Errors.Count);
            return result;
        }

        private async Task EnsureGateNoOverlapAsync(int gateId, DateTime departure, DateTime arrival, int? existingScheduleId = null)
        {
            var hasOverlap = await _unitOfWork.FlightSchedules.CheckGateOverlapAsync(gateId, departure, arrival, existingScheduleId);

            if (hasOverlap)
            {
                throw new GateOverlapException($"Gate {gateId} has overlapping schedule from {departure:u} to {arrival:u}"
                );
            }
        }

    }
}
