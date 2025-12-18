using AirportTool.Application.DTOs.FlightSchedule;
using AirportTool.Application.Exceptions;
using AirportTool.Application.Interfaces;
using AirportTool.Application.Interfaces.ServiceInterfaces;
using AirportTool.Domain.Entities;
using AutoMapper;

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

        public async Task<IEnumerable<FlightScheduleImportRowDto>> ImportSchedulesAsync(IEnumerable<FlightScheduleImportRowDto> schedules, CancellationToken cancellationToken = default)
        {
            var results = new List<FlightScheduleImportRowDto>();

            foreach (var dto in schedules)
            {
                try
                {
                    var originAirport = await _unitOfWork.Airports.GetByIataCodeAsync(dto.OriginIata, cancellationToken);
                    var destinationAirport = await _unitOfWork.Airports.GetByIataCodeAsync(dto.DestinationIata, cancellationToken);

                    if (originAirport == null || destinationAirport == null)
                    {
                        results.Add(dto);
                        continue;
                    }

                    var flights = await _unitOfWork.Flights.GetFlightsByRouteAsync(
                        originAirport.Id,
                        destinationAirport.Id,
                        dto.ScheduledDepartureUtc,
                        cancellationToken);

                    if (!flights.Any())
                    {
                        results.Add(dto);
                        continue;
                    }

                    var entity = _mapper.Map<FlightSchedule>(dto);

                    bool hasOverlap = await _unitOfWork.FlightSchedules.CheckGateOverlapAsync(
                        entity.GateId ?? 0,
                        entity.ScheduledDepartureUtc,
                        entity.ScheduledArrivalUtc,
                        null,
                        cancellationToken);

                    if (hasOverlap)
                    {
                        results.Add(dto);
                        continue;
                    }

                    await _unitOfWork.FlightSchedules.AddAsync(entity);
                    results.Add(dto);
                }
                catch
                {
                    results.Add(dto);
                }
            }

            await _unitOfWork.CompleteAsync();
            return results;
        }

    }
}
