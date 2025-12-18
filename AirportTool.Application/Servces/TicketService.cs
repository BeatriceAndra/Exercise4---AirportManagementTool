using AirportTool.Application.DTOs.Ticket;
using AirportTool.Application.Exceptions;
using AirportTool.Application.Interfaces;
using AirportTool.Application.Interfaces.Repositories;
using AirportTool.Application.Interfaces.ServiceInterfaces;
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

        public async Task<TicketReadDto> GetByIdAsync(int ticketId, CancellationToken cancellationToken = default)
        {
            var ticket = await _unitOfWork.Tickets.GetByIdAsync(ticketId);

            if (ticket == null)
            {
                throw new NotFoundException(nameof(ticket), ticketId);
            }

            return _mapper.Map<TicketReadDto>(ticket);
        }

        public async Task<IEnumerable<TicketReadDto>> GetTicketsByBookingAsync(int bookingId, CancellationToken cancellationToken = default)
        {
            var tickets = await _unitOfWork.Tickets.GetTicketsByBookingAsync(bookingId, cancellationToken);

            return tickets.Select(t => _mapper.Map<TicketReadDto>(t));
        }
    }
}
