using InsuranceProducts.Tests.Domain.Products.Entities;
using InsuranceProducts.Tests.Domain.Products.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceProducts.Tests.Infrastructure.Persistence.Configurations;

public class CoverageLevelTypeConfiguration : IEntityTypeConfiguration<CoverageLevelType>
{
    public void Configure(EntityTypeBuilder<CoverageLevelType> builder)
    {
        builder.HasKey(cov => cov.Id);

        // Configure Code value object
        builder.OwnsOne(cov => cov.Code, codeBuilder =>
        {
            codeBuilder.WithOwner();

            codeBuilder.Property(code => code.Value)
                .HasColumnName(nameof(CoverageType.Code))
                .HasConversion(ValueConverters.UpperConverter)
                .HasMaxLength(Code.MaxLength)
                .IsRequired();

            // Add index for title searches
            codeBuilder.HasIndex(code => code.Value)
                .HasDatabaseName("IX_CoverageLevelTypes_Code")
                .IsUnique();
        });

        // Configure Description value object
        builder.OwnsOne(pbi => pbi.Description, descBuilder =>
        {
            descBuilder.WithOwner();

            descBuilder.Property(d => d.Value)
                .HasColumnName(nameof(CoverageLevelType.Description))
                .HasMaxLength(Description.MaxLength)
                .IsRequired(false);
        });
    }
}
