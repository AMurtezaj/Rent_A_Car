using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.EntityConfigurations
{
    public class BookingOfferConfiguration : IEntityTypeConfiguration<BookingOffer>
    {
        public void Configure(EntityTypeBuilder<BookingOffer> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Booking)
                .WithMany(x => x.BookingOffers)
                .HasForeignKey(x => x.BookingId);

            builder.HasOne(x => x.Offer)
                .WithMany(x => x.BookingOffers)
                .HasForeignKey(x => x.OfferId);

            builder.Property(x => x.Quantity)
                .IsRequired();
        }

    }
}
