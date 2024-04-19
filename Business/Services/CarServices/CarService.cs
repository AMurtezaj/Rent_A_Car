using AutoMapper;
using Repositories.Repositories.CarRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Business.Services.FileHandlingServices;
using Data.DTOs.CarDtos;
using Data.Entities;
using Serilog;
using Org.BouncyCastle.Asn1.Crmf;
using Microsoft.AspNetCore.Http;
using Azure;
using Business.Utility;

namespace Business.Services.CarServices
{
    public class CarService : ICarService
    {

        private readonly ICarRepository _carRepository;
        private readonly IMapper _mapper;
        private readonly IBlobService _blobService;


        public CarService(ICarRepository carRepository, IMapper mapper, IBlobService blobService)
        {
            _carRepository = carRepository;
            _mapper = mapper;
            _blobService = blobService;
        }

        public ApiResponse<IList<CarDto>> GetAll()
        {
            try
            {
                var cars = _carRepository.GetAll();

                var carDtos = _mapper.Map<IList<CarDto>>(cars);
                return new ApiResponse<IList<CarDto>>()
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = carDtos
                };

            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, "An error occurred: {ErrorMessage}", ex.Message);
                return new ApiResponse<IList<CarDto>>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Errors = new List<string> { "An error occurred while processing your request. Please try again later." }
                };
            }
        }

        public ApiResponse<CarDto> GetCar(int id)
        {
            try
            {
                var car = _carRepository.Get(id);
                if (car == null)
                {
                    return new ApiResponse<CarDto>()
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Errors = new List<string>() { "The car was not found" }
                    };
                }
                return new ApiResponse<CarDto>()
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = _mapper.Map<CarDto>(car)
                };
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, "An error occurred: {Error Message}", ex.Message);
                return new ApiResponse<CarDto>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Errors = new List<string> { "An error occurred while processing your request. Please try again later" }
                };
                
            }
        }

        public ApiResponse<string> DeleteCar(int id)
        {
            try
            {
                var car = _carRepository.Get(id);
                if (car == null)
                {
                    return new ApiResponse<string>()
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Errors = new List<string> { "The car doesn't exist" }
                    };         
                }

                if (!string.IsNullOrEmpty(car.Image))
                {
                    _blobService.DeleteBlob(car.Image, SD.SD_Storage_Container);
                }

                if (_carRepository.Remove(car))
                {
                    return new ApiResponse<string>()
                    {
                        StatusCode = HttpStatusCode.OK,
                        Message = "The car was deleted successfully"
                    };
                }

                return new ApiResponse<string>()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Errors = new List<string>() { "The car was not deleted. Try again." }
                };
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, "An error occurred: {Error Message}", ex.Message);
                return new ApiResponse<string>()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Errors = new List<string> { "An error occurred while procesing your request. Please try again later" }
                };
            }
        }



        public async Task<ApiResponse<CarDto>> CreateCar(CarCreateDto carCreateDto)
        {
            var response = new ApiResponse<CarDto>();

            try
            {
                if (carCreateDto == null)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Errors = new List<string>() { "Invalid car data or no file provided." };
                    return response;
                }

                string fileName = $"{Guid.NewGuid()}{Path.GetExtension(carCreateDto.File.FileName)}";

                if (string.IsNullOrEmpty(fileName))
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Errors = new List<string>() { "Failed to upload the image to Azure Blob Storage." };
                    return response;
                }

                var car = new Car
                {
                    Model = carCreateDto.Model,
                    Year = carCreateDto.Year,
                    Speed = carCreateDto.Speed,
                    Color = carCreateDto.Color,
                    TransmissionType = carCreateDto.TransmissionType,
                    FuelType = carCreateDto.FuelType,
                    Seats = carCreateDto.Seats,
                    PricePerDay = carCreateDto.PricePerDay,
                    Available = true, // Assuming the newly created car is available by default
                    Description = carCreateDto.Description,
                    Image = await _blobService.UploadBlob(fileName, SD.SD_Storage_Container, carCreateDto.File)
                };

                // Add the car to the repository
                if (_carRepository.Add(car))
                {
                    response.StatusCode = HttpStatusCode.Created;
                    response.Data = _mapper.Map<CarDto>(car);
                    return response;
                }
                else
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Errors = new List<string>() { "Failed to save the car. Please try again." };
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





        public ApiResponse<IList<CarForDisplayDto>> GetCarForDisplay()
        {
            try
            {
                var cars = _carRepository.CarForDisplayDto();

                var carDtos = _mapper.Map<IList<CarForDisplayDto>>(cars);

                return new ApiResponse<IList<CarForDisplayDto>>()
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = carDtos
                };
            }
            catch (Exception e)
            {
                Log.Error(e.Message, "An error occurred: {ErrorMessage}", e.Message);
                return new ApiResponse<IList<CarForDisplayDto>>()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Errors = new List<string> { "An error occurred while procesing your request. Please try again later" }
                };
            }
            
        }


        public async Task<ApiResponse<CarDto>> EditCar(CarUpdateDto carUpdateDto)
        {
            var response = new ApiResponse<CarDto>();

            try
            {
                if (carUpdateDto == null)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Errors = new List<string> { "Invalid car data or no file provided " };
                    return response;
                }

                var carInDb = _carRepository.Get(carUpdateDto.Id);
                if (carInDb == null)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Errors = new List<string>() { "The car doesn't exist" };
                    return response;
                }

                string fileName = null;
                if (carUpdateDto.File != null && carUpdateDto.File.Length > 0)
                { 
                    fileName = $"{Guid.NewGuid()}{Path.GetExtension(carUpdateDto.File.FileName)}";
                    carInDb.Image = await _blobService.UploadBlob(fileName, SD.SD_Storage_Container, carUpdateDto.File);
                }

                carInDb.Model = carUpdateDto.Model;
                carInDb.Year = carUpdateDto.Year;
                carInDb.Speed = carUpdateDto.Speed;
                carInDb.Color = carUpdateDto.Color;
                carInDb.TransmissionType = carUpdateDto.TransmissionType;
                carInDb.FuelType = carUpdateDto.FuelType;
                carInDb.Seats = carUpdateDto.Seats;
                carInDb.PricePerDay = carUpdateDto.PricePerDay;
                carInDb.Available = carUpdateDto.Available;
                carInDb.Description = carUpdateDto.Description;

                if (_carRepository.Update(carInDb))
                {
                    response.StatusCode = HttpStatusCode.OK;
                    response.Data = _mapper.Map<CarDto>(carInDb);
                    return response;
                }
                else
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Errors = new List<string>() { "Failed to update the car. Please try again" };
                    return response;
                }

            }
            catch (Exception e)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Errors = new List<string>() { "An error occurred while processing your request. Please try again " };
                return response;
            }
        }

    }
}
