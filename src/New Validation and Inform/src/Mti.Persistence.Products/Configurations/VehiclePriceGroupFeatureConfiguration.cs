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

internal sealed class VehiclePriceGroupFeatureConfiguration 
    : IEntityTypeConfiguration<VehiclePriceGroupFeature>
{
    public void Configure(EntityTypeBuilder<VehiclePriceGroupFeature> builder)
    {
        // Configure Code value object
        builder.Property(e => e.Code)
        .HasConversion(
            e => e.Value.ToUpperInvariant(),
            v => Code.Create(v.ToUpperInvariant())
        )
        .HasMaxLength(Code.MaxLength)
        .IsRequired();

        // Configure Name value object
        builder.Property(e => e.Name)
        .HasConversion(
            e => e.Value,
            v => Name.Create(v)
        )
        .HasMaxLength(Name.MaxLength)
        .IsRequired();

        // Create index for UnitCategory foreign key
        builder.HasIndex(nameof(VehiclePriceGroupFeature.Code))
            .HasDatabaseName("IX_VehiclePriceGroupFeatures_Code")
            .IsUnique();
    }
}
