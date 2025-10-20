using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mti.Domain.Products.Entities;
using Mti.Domain.Products.ValueObjects;

namespace Mti.Persistence.Products.Configurations;

internal class VehicleBrandFeatureConfiguration 
    : IEntityTypeConfiguration<VehicleBrandFeature>
{
    public void Configure(EntityTypeBuilder<VehicleBrandFeature> builder)
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
        builder.HasIndex(nameof(VehicleBrandFeature.Code))
            .HasDatabaseName("IX_VehicleBrandFeatures_Code")
            .IsUnique();
    }
}
