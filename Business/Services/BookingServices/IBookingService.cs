using Data.DTOs.BookingDtos;
using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.BookingServices
{
    public interface IBookingService
    {
        ApiResponse<BookingDto> GetBooking(int id);
        ApiResponse<IList<BookingDto>> GetAllBookings();
        ApiResponse<string> DeleteBooking(int id);
        Task<ApiResponse<BookingDto>> CreateBooking(BookingCreateDto bookingCreateDto);
    }
}
