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
    public class CarBookingConfiguration : IEntityTypeConfiguration<CarBooking>
    {
        public void Configure(EntityTypeBuilder<CarBooking> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x=>x.Car)
                .WithMany(x=>x.CarBookings)
                .HasForeignKey(x=>x.CarId);

            builder.HasOne(x=>x.Booking)
                .WithMany(x=>x.CarBookings)
                .HasForeignKey(x=>x.BookingId);
        }

    }
}
