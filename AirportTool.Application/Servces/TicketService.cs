using AirportTool.Application.DTOs.Ticket;
using AirportTool.Application.Exceptions;
using AirportTool.Application.Interfaces;
using AirportTool.Application.Interfaces.ServiceInterfaces;
using AirportTool.Domain.Entities;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace AirportTool.Application.Services
{
    public class TicketService : ITicketService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<TicketService> _logger;

        public TicketService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<TicketService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;

        }

        public async Task<List<TicketReadDto>> GetTicketsByFlightScheduleIdAsync(int flightScheduleId)
        {
            var allTickets = await _unitOfWork.Tickets.GetAllAsync();
            var tickets = allTickets.Where(t => t.FlightScheduleId == flightScheduleId).ToList();

            if (!tickets.Any())
                throw new NotFoundException("Tickets", flightScheduleId);

            var flightSchedule = await _unitOfWork.FlightSchedules.GetByIdAsync(flightScheduleId);

            if (flightSchedule == null)
                throw new NotFoundException("FlightSchedule", flightScheduleId);

            if (!flightSchedule.AssignedAircraftId.HasValue)
                throw new Exception("Flight schedule does not have an assigned aircraft.");

            var aircraft = await _unitOfWork.Aircrafts.GetByIdAsync(flightSchedule.AssignedAircraftId.Value);

            if (aircraft == null)
                throw new NotFoundException("Aircraft", flightSchedule.AssignedAircraftId);

            var capacity = aircraft.SeatCapacity;

            var bookings = await _unitOfWork.Bookings.GetAllAsync();
            var soldSeats = bookings.Sum(b => b.Quantity);

            var seatsAvailable = capacity - soldSeats;

            var result = _mapper.Map<List<TicketReadDto>>(tickets);

            foreach (var dto in result)
                dto.SeatsAvailable = seatsAvailable;

            return result;
        }

        public async Task<TicketReadDto> CreateTicketAsync(TicketCreateDto dto)
        {
            var ticket = _mapper.Map<Ticket>(dto);

            await _unitOfWork.Tickets.AddAsync(ticket);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Ticket created. TicketId={TicketId}, FlightScheduleId={ScheduleId}, Price={Price}", ticket.Id, ticket.FlightScheduleId, ticket.TotalPrice);
            
            return _mapper.Map<TicketReadDto>(ticket);

        }

        public async Task DeleteTicketAsync(int ticketId)
        {
            var ticket = await _unitOfWork.Tickets.GetByIdAsync(ticketId);
            if (ticket == null)
                throw new NotFoundException(nameof(Ticket), ticketId);

            await _unitOfWork.Tickets.DeleteAsync(ticketId);
            await _unitOfWork.CompleteAsync();

            _logger.LogWarning("Ticket deleted. TicketId={TicketId}", ticketId);

        }
    }

}
