using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Cart
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public IList<CartOffer>? CartOffers { get; set; }
        public float? Total { get; set; }
    }
}
