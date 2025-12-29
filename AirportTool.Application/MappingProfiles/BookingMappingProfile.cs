using AirportTool.Application.DTOs.Booking;
using AirportTool.Domain.Entities;
using AirportTool.Domain.Enums;
using AutoMapper;

namespace AirportTool.Application.MappingProfiles;

public class BookingMappingProfile : Profile
{
    public BookingMappingProfile()
    {
        // --- CreateBookingDto -> Booking ---
        CreateMap<BookingCreateDto, Booking>()
            .ForMember(d => d.ConfirmationCode, o => o.Ignore())
            .ForMember(d => d.CreatedUtc, o => o.MapFrom(_ => DateTime.UtcNow))
            .ForMember(d => d.Tickets, o => o.Ignore())
            .ForMember(d => d.BookingStatusId, o => o.Ignore())
            .ForMember(d => d.UserId, o => o.Ignore());

        // --- Booking -> BookingReadDto ---
        CreateMap<Booking, BookingReadDto>()
            .ForMember(d => d.ConfirmationCode, o => o.MapFrom(src => src.ConfirmationCode))
            .ForMember(d => d.Status, o => o.MapFrom(src => BookingStatus.Active.ToString()))
            .ForMember(d => d.TotalAmount, o => o.MapFrom(src => src.Tickets.Sum(t => t.TotalPrice)));

    }
}
