using Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs.BookingDtos
{
    public class BookingUpdateDto
    {
        public int Id { get; set; }
        public DateTime PickUpDateTime { get; set; }
        public DateTime ReturnDateTime { get; set; }
        public double TotalPrice { get; set; }
        public BookingStatus BookingStatuses { get; set; }
        public int UserId { get; set; }
        public int CarId { get; set; }
    }
}
