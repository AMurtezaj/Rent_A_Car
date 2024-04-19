using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class CartOffer
    {
        public int Id { get; set; }
        public int? OfferId { get; set; }
        public int? CartId { get; set; }
        public int Quantity { get; set; }
        public virtual Offer Offer { get; set; }
        public virtual Cart Cart { get; set; }
    }
}
