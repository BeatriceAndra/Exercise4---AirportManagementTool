using AirportTool.Application.DTOs.Ticket;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AirportTool.Application.Interfaces.ServiceInterfaces
{
    public interface ITicketService
    {
        Task<TicketReadDto> GetByIdAsync(int ticketId, CancellationToken cancellationToken = default);
        Task<IEnumerable<TicketReadDto>> GetTicketsByBookingAsync(int bookingId, CancellationToken cancellationToken = default);
    }
}
