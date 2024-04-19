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
    public interface IOfferRepository : IRepository<Offer>
    {
        IList<OfferForDisplayDto> OfferForDisplay();
    }
}
