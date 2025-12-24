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
        Task<BookingReadDto> CreateBookingAsync(BookingCreateDto dto, int userId);
        Task<BookingReadDto> GetByConfirmationCodeAsync(string confirmationCode);
        Task CancelBookingAsync(string confirmationCode);
    }
}
