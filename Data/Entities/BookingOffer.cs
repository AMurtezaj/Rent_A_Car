using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class BookingOffer
    {
        public int Id { get; set; }
        public int? BookingId { get; set; }
        public int? OfferId { get; set; }
        public int? Quantity { get; set; }
        public Booking Booking { get; set; }
        public Offer Offer { get; set; }

    }
}
