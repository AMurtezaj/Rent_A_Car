using AutoMapper;
using Business.Services.FileHandlingServices;
using Data.DTOs.BookingDtos;
using Data.Entities;
using Repositories;
using Repositories.Repositories.BookingRepositories;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.BookingServices
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;
        private readonly IBlobService _blobService;

        public BookingService(IBookingRepository bookingRepository, IMapper mapper, IBlobService blobService)
        { 
            _bookingRepository = bookingRepository;
            _mapper = mapper;
            _blobService = blobService;
        }

        public ApiResponse<BookingDto> GetBooking(int id)
        {
            var response = new ApiResponse<BookingDto>();
            try
            {
                var booking = _bookingRepository.Get(id);
                if (booking == null)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Errors = new List<string>() { "Booking not found" };
                    return response;
                }

                var bookingDto = _mapper.Map<BookingDto>(booking);

                response.StatusCode = HttpStatusCode.OK;
                response.Data = bookingDto; 
            }
            catch (Exception e)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Errors = new List<string>() { "An error occurred while processing your request. Please try again" };
            }
            return response;
        }

        public ApiResponse<IList<BookingDto>> GetAllBookings()
        {
            try
            {
                var bookings = _bookingRepository.GetAll();

                var bookingDtos = _mapper.Map<IList<BookingDto>>(bookings);
                return new ApiResponse<IList<BookingDto>>()
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = bookingDtos
                };

            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, "An error occurred: {ErrorMessage}", ex.Message);
                return new ApiResponse<IList<BookingDto>>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Errors = new List<string> { "An error occurred while processing your request. Please try again later." }
                };
            }
        }

        public ApiResponse<string> DeleteBooking(int id)
        {
            try
            {
                var booking = _bookingRepository.Get(id);
                if (booking == null)
                {
                    return new ApiResponse<string>()
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Errors = new List<string> { "The booking doesn't exist" }
                    };
                
                }

                if (_bookingRepository.Remove(booking))
                {
                    return new ApiResponse<string>()
                    {
                        StatusCode = HttpStatusCode.OK,
                        Message = "The booking was deleted successfully"
                    };
                }
                return new ApiResponse<string>()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Errors = new List<string>() { "The booking was not deleted. Try again" }
                };
            }
            catch (Exception e)
            {
                Log.Error(e.Message, "An error occurred: {Error Message}", e.Message);
                return new ApiResponse<string>()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Errors = new List<string> { "An error occurred while processing your request. Please try again" }
                };
            }
        }

        public async Task<ApiResponse<BookingDto>> CreateBooking(BookingCreateDto bookingCreateDto)
        {
            var response = new ApiResponse<BookingDto>();
            try
            {
                if (bookingCreateDto == null)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Errors = new List<string>() { "Invalid booking data or no file provided" };
                }

                var booking = new Booking
                {
                    PickUpDateTime = bookingCreateDto.PickUpDateTime,
                    ReturnDateTime = bookingCreateDto.ReturnDateTime,
                    TotalPrice = bookingCreateDto.TotalPrice,
                    BookingStatuses = bookingCreateDto.BookingStatuses,
                    UserId = bookingCreateDto.UserId,
                    CarId = bookingCreateDto.CarId
                };

                if (_bookingRepository.Add(booking))
                {
                    response.StatusCode = HttpStatusCode.Created;
                    response.Data = _mapper.Map<BookingDto>(booking);
                    return response;
                }
                else
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Errors = new List<string>() { "Failed to save the booking. Please try again" };
                    return response;
                }

            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Errors = new List<string>() { $"An error occurred: {ex.Message}" };
                return response;

            }
        }
    }
}
