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
    public class InsuranceOptionConfiguration : IEntityTypeConfiguration<InsuranceOption>
    {

        public void Configure(EntityTypeBuilder<InsuranceOption> builder)
        {
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Description).IsRequired();
            builder.Property(x=>x.Price).IsRequired().IsRequired();

            builder.HasOne(e => e.Car)
                .WithMany(e=>e.InsuranceOptions)
                .HasForeignKey(e=>e.CarId).OnDelete(DeleteBehavior.Cascade);
            
        }


    }
}
