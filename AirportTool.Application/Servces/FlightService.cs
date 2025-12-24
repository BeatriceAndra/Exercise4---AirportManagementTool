using AirportTool.Application.DTOs.Flight;
using AirportTool.Application.Exceptions;
using AirportTool.Application.Interfaces;
using AirportTool.Application.Interfaces.Repositories;
using AirportTool.Application.Interfaces.ServiceInterfaces;
using AirportTool.Domain.Entities;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace AirportTool.Application.Services
{
    public class FlightService : IFlightService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public FlightService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<FlightService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<FlightReadDto> GetFlightWithSchedulesAsync(int flightId)
        {
            var flight = await _unitOfWork.Flights.GetFlightWithSchedulesAsync(flightId);

            if (flight == null)
            {
                throw new NotFoundException(nameof(GetFlightWithSchedulesAsync), flightId);
            }

            return _mapper.Map<FlightReadDto>(flight);

        }

        public async Task<IEnumerable<FlightReadDto>> GetFlightsByRouteAsync(int originAirportId, int destinationAirportId, DateTime? date = null)
        {
            var flights = await _unitOfWork.Flights.GetFlightsByRouteAsync(originAirportId, destinationAirportId, date);

            if (!flights.Any())
            {
                throw new NotFoundException(nameof(GetFlightsByRouteAsync), $"No flights found for route {originAirportId} -> {destinationAirportId}");
            }

            return _mapper.Map<IEnumerable<FlightReadDto>>(flights);

        }

        public async Task<FlightReadDto> CreateFlightAsync(FlightCreateDto dto)
        {
            var flight = _mapper.Map<Flight>(dto);

            await _unitOfWork.Flights.AddAsync(flight);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Flight created. FlightId={FlightId}, FlightNumber={FlightNumber}", flight.Id, flight.FlightNumber);

            return _mapper.Map<FlightReadDto>(flight);

        }

        public async Task UpdateFlightAsync(int id, FlightUpdateDto dto)
        {
            var flight = await _unitOfWork.Flights.GetByIdAsync(id);
            if (flight == null)
            {
                throw new NotFoundException(nameof(Flight), id);
            }
            _mapper.Map(dto, flight);

            await _unitOfWork.Flights.UpdateAsync(flight);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Flight updated. FlightId={FlightId}", flight.Id);

        }

        public async Task DeleteFlightAsync(int id)
        {
            var flight = await _unitOfWork.Flights.GetByIdAsync(id);
            if (flight == null)
                throw new NotFoundException(nameof(Flight), id);

            await _unitOfWork.Flights.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();

            _logger.LogWarning("Flight deleted. FlightId={FlightId}",flight.Id);

        }
    }
}
