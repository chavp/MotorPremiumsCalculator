using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mti.Domain.Products.Entities;

namespace Mti.Persistence.Products.Configurations;

internal sealed class InsuranceRateConfiguration 
    : IEntityTypeConfiguration<InsuranceRate>
{
    public void Configure(EntityTypeBuilder<InsuranceRate> builder)
    {
        builder.HasKey(ca => ca.Id);

        // Configure relationships
        builder.HasOne(ca => ca.Product)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.HasOne(ca => ca.CoverageType)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);

        builder.HasOne(ca => ca.CoverageLevel)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);

        builder.HasOne(ca => ca.ProductFeature)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);

        builder.HasOne(ca => ca.Unit)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.HasOne(ca => ca.PeriodType)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        // Configure indexes
        builder.HasIndex(ca => new {
            ca.ProductId,
            ca.CoverageTypeId,
            ca.CoverageLevelId,
            ca.ProductFeatureId,
            ca.UnitId,
            ca.PeriodTypeId,
            ca.EffectiveDate
        })
        .HasDatabaseName("IX_InsuranceRates_ProductFeatures_Coverages")
        .IsUnique();

    }
}
