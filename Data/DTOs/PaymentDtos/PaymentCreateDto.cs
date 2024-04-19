using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs.PaymentDtos
{
    public class PaymentCreateDto
    {
        public double Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentMethod { get; set; }
        public int UserId { get; set; }
        public int BookingId { get; set; }
    }
}
