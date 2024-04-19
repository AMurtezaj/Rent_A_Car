using AutoMapper;
using Data.DTOs.BookingDtos;
using Data.DTOs.CarDtos;
using Data.Entities;

namespace API

{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Car, CarDto>();
            CreateMap<CarDto, Car>();
            CreateMap<CarCreateDto, Car>();
            CreateMap<Car, CarCreateDto>();


        }
    }


}