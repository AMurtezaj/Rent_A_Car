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
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.Property(x => x.Rating);
            builder.Property(x => x.DatePosted);
            builder.Property(x => x.Comment);

            builder.HasOne(x=>x.User)
                .WithMany(x=>x.Reviews)
                .HasForeignKey(x=>x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Car)
                .WithMany(x => x.Reviews)
                .HasForeignKey(x => x.CarId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
