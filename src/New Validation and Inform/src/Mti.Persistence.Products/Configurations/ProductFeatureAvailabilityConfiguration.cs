using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mti.Domain.Products.Entities;

namespace Mti.Persistence.Configurations;

internal sealed class ProductFeatureAvailabilityConfiguration
    : IEntityTypeConfiguration<ProductFeatureAvailability>
{
    public void Configure(EntityTypeBuilder<ProductFeatureAvailability> builder)
    {
        builder.HasKey(pf => pf.Id);

        // Configure relationships
        builder.HasOne(pf => pf.Product)
            .WithMany(p => p.ProductFeatureAvailabilities)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.HasOne(pf => pf.ProductFeatureAvailabilityType)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.HasOne(pf => pf.ProductFeature)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        // Configure indexes
        builder.HasIndex(pf => new {
            pf.ProductId,
            pf.ProductFeatureAvailabilityTypeId,
            pf.ProductFeatureId,
        })
        .HasDatabaseName("IX_ProductFeatureAvailabilities_ProductFeatures");

    }
}
