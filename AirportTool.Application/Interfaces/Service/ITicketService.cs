using AirportTool.Application.DTOs.Ticket;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AirportTool.Application.Interfaces.ServiceInterfaces
{
    public interface ITicketService
    {
        Task<IEnumerable<TicketReadDto>> GetTicketsByFlightAsync(int flightId);
        Task<TicketReadDto> CreateTicketAsync(TicketCreateDto dto);
        Task DeleteTicketAsync(int ticketId);
    }
}
