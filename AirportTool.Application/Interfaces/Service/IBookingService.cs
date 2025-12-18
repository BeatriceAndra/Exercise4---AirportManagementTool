using AirportTool.Application.DTOs.Booking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTool.Application.Interfaces.ServiceInterfaces
{
    public interface IBookingService
    {
        Task<BookingReadDto> CreateBookingAsync(BookingCreateDto dto, int userId, CancellationToken cancellationToken = default);
        Task<BookingReadDto> GetByConfirmationCodeAsync(string confirmationCode, CancellationToken cancellationToken = default);
        Task CancelBookingAsync(string confirmationCode, CancellationToken cancellationToken = default);
    }
}
