using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs.CarDtos
{
    public class CarForDisplayDto
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public int Speed { get; set; }
        public string Color { get; set; }
        public string TransmissionType { get; set; }
        public string FuelType { get; set; }
        public int Seats { get; set; }
        public double PricePerDay { get; set; }
        public bool Available { get; set; }
        public string Description { get; set; }
        public string? Image { get; set; }

    }
}
