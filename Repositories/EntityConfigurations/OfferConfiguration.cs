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
    public class OfferConfiguration : IEntityTypeConfiguration<Offer>
    {
        public void Configure(EntityTypeBuilder<Offer> builder)
        {
            builder.Property(e => e.Name)
                .IsRequired();
            builder.Property(e => e.Price)
                .IsRequired();
            builder.Property(e => e.StartDate)
                .IsRequired();
            builder.Property(e => e.EndDate)
                .IsRequired();
            builder.Property(e => e.DiscountPercent)
                .IsRequired();
            builder.Property(e => e.Image)
                .IsRequired(false);


            builder.HasOne(e => e.Car)
                .WithMany(e => e.Offers)
                .HasForeignKey(e => e.CarId)
                .IsRequired();

        }
    }
}
