using Data.DTOs.OfferDtos;
using Data.Entities;
using Repositories.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories.OfferRepositories
{
    public class OfferRepository : Repository<Offer>, IOfferRepository
    {
        public OfferRepository(AppDbContext context) : base(context)
        { 
        
        }

        public IList<OfferForDisplayDto> OfferForDisplay()
        {
            return Context.Set<Offer>().Select(x =>
                new OfferForDisplayDto()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Image = x.Image,
                    Price = x.Price,
                    DiscountPercent = x.DiscountPercent,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                }

            ).ToList();
        }

    }
}
