using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mti.Domain.Products.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mti.Persistence.Products.Configurations;

internal sealed class VehicleSizeConfiguration
     : IEntityTypeConfiguration<VehicleSize>
{
    public void Configure(EntityTypeBuilder<VehicleSize> builder)
    {
        builder.HasKey(e => e.Id);

        // Configure relationships
        builder.HasOne(e => e.VehicleTypeVoluntary)
            .WithMany(vehVo => vehVo.VehicleSizes)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.HasOne(e => e.Unit)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

    }
}
