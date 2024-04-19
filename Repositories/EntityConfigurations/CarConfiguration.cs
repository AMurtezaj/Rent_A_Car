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
    public class CarConfiguration :IEntityTypeConfiguration<Car>
    {
        public void Configure(EntityTypeBuilder<Car> builder)
        {
            builder.Property(x => x.Model);
            builder.Property(x => x.FuelType);
            builder.Property(x => x.Speed);
            builder.Property(x => x.TransmissionType);
            builder.Property(x => x.PricePerDay);
            builder.Property(x => x.Seats);
            builder.Property(x => x.Available);
            builder.Property(x => x.Image);


            builder.HasMany(x => x.Reviews)
                .WithOne(x => x.Car)
                .HasForeignKey(x => x.CarId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Bookings)
                .WithOne(x => x.Car)
                .HasForeignKey(x => x.CarId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.InsuranceOptions)
                .WithOne(x => x.Car)
                .HasForeignKey(x => x.CarId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Offers)
                .WithOne(x => x.Car)
                .HasForeignKey(x => x.CarId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
