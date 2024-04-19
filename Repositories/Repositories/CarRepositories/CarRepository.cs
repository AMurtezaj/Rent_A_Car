using Data.DTOs.CarDtos;
using Data.Entities;
using Repositories.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace Repositories.Repositories.CarRepositories
{
    public class CarRepository : Repository<Car>, ICarRepository
    {
        public CarRepository(AppDbContext context) : base(context)
        { 
        
        }

        public IList<CarForDisplayDto> CarForDisplayDto()
        { 
        return Context.Set<Car>().Select(x=>
            new CarForDisplayDto()
            { 
                Id = x.Id,
                Model = x.Model,
                Year = x.Year,
                Speed = x.Speed,
                Color = x.Color,
                TransmissionType = x.TransmissionType,
                FuelType = x.FuelType,
                Seats = x.Seats,
                PricePerDay = x.PricePerDay,
                Available = x.Available,
                Description = x.Description,
            }
            ).ToList();
        }
    }
}
