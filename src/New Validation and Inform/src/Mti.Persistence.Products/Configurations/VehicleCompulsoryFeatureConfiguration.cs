using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mti.Domain.Products.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mti.Persistence.Products.Configurations;

internal sealed class VehicleCompulsoryFeatureConfiguration 
    : IEntityTypeConfiguration<VehicleCompulsoryFeature>
{
    public void Configure(EntityTypeBuilder<VehicleCompulsoryFeature> builder)
    {
        // Create index for VehicleFuelType foreign key
        builder.HasIndex(nameof(VehicleCompulsoryFeature.VehicleTypeVoluntaryId)
                , nameof(VehicleCompulsoryFeature.Min), nameof(VehicleCompulsoryFeature.UnitId)
                , nameof(VehicleCompulsoryFeature.VehicleTypeCompulsoryId))
            .HasDatabaseName("IX_VehicleCompulsoryFeatures_VehicleTypeVoluntaryId_Min_UnitId_VehicleTypeCompulsoryId")
            .IsUnique();
    }
}
