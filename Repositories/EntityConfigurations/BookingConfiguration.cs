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
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {

        public void Configure(EntityTypeBuilder<Booking> builder)
        { 
            builder.Property(x=>x.PickUpDateTime)
                .IsRequired();
            builder.Property(x => x.ReturnDateTime)
                .IsRequired();
            builder.Property(x => x.TotalPrice)
                .IsRequired();
            builder.Property(x => x.BookingStatuses)
                .IsRequired();

            builder.HasOne(x => x.Car)
                .WithMany(x => x.Bookings)
                .HasForeignKey(x => x.CarId);

        }
    }
}
