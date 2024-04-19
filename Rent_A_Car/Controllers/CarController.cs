using Business.Services.CarServices;
using Data.DTOs.CarDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Net;

namespace Rent_A_Car.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly ICarService _carService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CarController(ICarService carService, IWebHostEnvironment webHostEnvironment)
        {
            _carService = carService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult GetAllCars()
        {
            var response = _carService.GetAll();
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet("{id}")]
        public IActionResult GetCar(int id)
        {
            var response = _carService.GetCar(id);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpDelete("{id}")]

        public IActionResult DeleteCar(int id)
        {
            var response = _carService.DeleteCar(id);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCar([FromForm] CarCreateDto carDto)
        {
            try
            {
                var response = await _carService.CreateCar(carDto);

                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, "An error occurred: {ErrorMessage}", ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request. Please try again later.");
            }
        }
        [HttpGet]

        public IActionResult GetCarForDisplay()
        {
            var response = _carService.GetCarForDisplay();
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPut]
        public async Task<IActionResult> EditCar([FromForm] CarUpdateDto carDto)
        {
            try
            {
                var response = await _carService.EditCar(carDto);

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
