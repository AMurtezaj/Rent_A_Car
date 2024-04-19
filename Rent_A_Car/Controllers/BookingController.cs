using Business.Services.BookingServices;
using Business.Services.CarServices;
using Data.DTOs.BookingDtos;
using Data.DTOs.CarDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Net;

namespace Rent_A_Car.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        { 
            _bookingService = bookingService;
        }

        [HttpGet("{id}")]
        public IActionResult GetBooking(int id)
        {
            var response = _bookingService.GetBooking(id);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking(BookingCreateDto bookingDto)
        {
            try
            {
                var response = await _bookingService.CreateBooking(bookingDto);

                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, "An error occurred: {ErrorMessage}", ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request. Please try again later.");
            }
        }


    }
}
