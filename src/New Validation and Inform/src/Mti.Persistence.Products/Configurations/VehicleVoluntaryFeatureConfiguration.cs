using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mti.Domain.Products.Entities;
using Mti.Domain.Products.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mti.Persistence.Products.Configurations;

internal sealed class VehicleVoluntaryFeatureConfiguration 
    : IEntityTypeConfiguration<VehicleVoluntaryFeature>
{
    public void Configure(EntityTypeBuilder<VehicleVoluntaryFeature> builder)
    {
        // Create index for VehicleFuelType foreign key
        builder.HasIndex(nameof(VehicleVoluntaryFeature.VehicleWorkshopTypeId), nameof(VehicleVoluntaryFeature.VehicleTypeVoluntaryId))
            .HasDatabaseName("IX_VehicleVoluntaryFeatures_VehicleWorkshopTypeId_VehicleTypeVoluntaryId")
            .IsUnique();
    }
}
