using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class CarBooking
    {
        public int Id { get; set; }
        public int? BookingId { get; set; }
        public int? CarId { get; set; }
        public Booking Booking { get; set; }
        public Car Car { get; set; }
    }
}
