using AirportTool.Application.DTOs.Ticket;
using AirportTool.Application.Exceptions;
using AirportTool.Application.Interfaces;
using AirportTool.Application.Interfaces.Repositories;
using AirportTool.Application.Interfaces.ServiceInterfaces;
using AirportTool.Domain.Entities;
using AutoMapper;

namespace AirportTool.Application.Services
{
    public class TicketService : ITicketService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TicketService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TicketReadDto>> GetTicketsByFlightAsync(int flightId)
        {
            var tickets = await _unitOfWork.Tickets.GetTicketsByFlightAsync(flightId);
            return _mapper.Map<IEnumerable<TicketReadDto>>(tickets);
        }

        public async Task<TicketReadDto> CreateTicketAsync(TicketCreateDto dto)
        {
            var ticket = _mapper.Map<Ticket>(dto);

            await _unitOfWork.Tickets.AddAsync(ticket);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<TicketReadDto>(ticket);
        }

        public async Task DeleteTicketAsync(int ticketId)
        {
            var ticket = await _unitOfWork.Tickets.GetByIdAsync(ticketId);
            if (ticket == null)
                throw new NotFoundException(nameof(Ticket), ticketId);

            await _unitOfWork.Tickets.DeleteAsync(ticketId);
            await _unitOfWork.CompleteAsync();
        }
    }

}
