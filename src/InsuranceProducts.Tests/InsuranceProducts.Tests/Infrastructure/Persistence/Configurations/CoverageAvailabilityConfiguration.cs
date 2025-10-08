using InsuranceProducts.Tests.Domain.Products.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceProducts.Tests.Infrastructure.Persistence.Configurations;

internal class CoverageAvailabilityConfiguration : IEntityTypeConfiguration<CoverageAvailability>
{
    public void Configure(EntityTypeBuilder<CoverageAvailability> builder)
    {
        builder.HasKey(ca => ca.Id);

        // Configure relationships
        builder.HasOne(ca => ca.Product)
            .WithMany(p => p.CoverageAvailabilities)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.HasOne(ca => ca.CoverageAvailabilityType)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.HasOne(ca => ca.CoverageType)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.HasOne(ca => ca.CoverageLevel)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        // Configure indexes
        builder.HasIndex(ca => new { 
            ca.ProductId, 
            ca.CoverageAvailabilityTypeId,
            ca.CoverageTypeId,
            ca.CoverageLevelId,
        })
        .HasDatabaseName("IX_CoverageAvailabilities_ProductCoverages");

    }
}
