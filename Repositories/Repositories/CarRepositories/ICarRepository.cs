using Data.DTOs.CarDtos;
using Data.Entities;
using Repositories.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories.CarRepositories
{
    public interface ICarRepository :IRepository<Car>
    {
        IList<CarForDisplayDto> CarForDisplayDto();
    }
}
