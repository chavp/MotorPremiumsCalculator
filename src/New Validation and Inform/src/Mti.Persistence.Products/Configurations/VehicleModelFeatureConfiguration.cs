using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mti.Domain.Products.Entities;
using Mti.Domain.Products.ValueObjects;

namespace Mti.Persistence.Products.Configurations;

internal class VehicleModelFeatureConfiguration 
    : IEntityTypeConfiguration<VehicleModelFeature>
{
    public void Configure(EntityTypeBuilder<VehicleModelFeature> builder)
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

        // Configure Name value object
        builder.Property(e => e.MtiCode)
        .HasColumnName("MtiCode")
        .HasConversion(
            e => e.Value,
            v => Code.Create(v)
        )
        .HasMaxLength(Code.MaxLength)
        .IsRequired(false);

        // Configure relationships
        builder.HasOne(v => v.VehicleBrandFeature)
           .WithMany(b => b.Models)
           .OnDelete(DeleteBehavior.Cascade)
           .IsRequired();

        builder.HasOne(v => v.VehiclePriceGroupFeature)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);


        // Create index foreign key
        builder.HasIndex(
            nameof(VehicleModelFeature.VehicleBrandFeatureId), nameof(VehicleModelFeature.Code))
            .HasDatabaseName("IX_VehicleModelFeatures_VehicleBrandId_Code")
            .IsUnique();
    }
}
