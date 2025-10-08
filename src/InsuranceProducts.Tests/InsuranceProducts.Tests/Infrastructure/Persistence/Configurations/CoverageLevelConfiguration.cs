using InsuranceProducts.Tests.Domain.Products.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceProducts.Tests.Infrastructure.Persistence.Configurations;

internal sealed class CoverageLevelConfiguration : IEntityTypeConfiguration<CoverageLevel>
{
    public void Configure(EntityTypeBuilder<CoverageLevel> builder)
    {
        builder.HasKey(cov => cov.Id);

        // Configure relationships
        builder.HasOne(cl => cl.CoverageLevelType)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.HasOne(cl => cl.CoverageBasis)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.HasOne(cl => cl.Unit)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}
