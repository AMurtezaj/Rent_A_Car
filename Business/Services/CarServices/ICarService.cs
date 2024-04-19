using Data.DTOs.CarDtos;
using Data.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.CarServices
{
    public interface ICarService
    {
        ApiResponse<IList<CarDto>> GetAll();
        ApiResponse<CarDto> GetCar(int id);
        ApiResponse<string> DeleteCar(int id);
        Task<ApiResponse<CarDto>> CreateCar(CarCreateDto carCreateDto);
        ApiResponse<IList<CarForDisplayDto>> GetCarForDisplay();
        Task<ApiResponse<CarDto>> EditCar(CarUpdateDto carUpdateDto);
    }
}
