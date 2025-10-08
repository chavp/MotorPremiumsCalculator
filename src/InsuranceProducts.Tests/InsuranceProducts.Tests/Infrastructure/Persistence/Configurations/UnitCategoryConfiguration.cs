using InsuranceProducts.Tests.Domain.Products.Entities;
using InsuranceProducts.Tests.Domain.Products.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceProducts.Tests.Infrastructure.Persistence.Configurations;

internal sealed class UnitCategoryConfiguration : IEntityTypeConfiguration<UnitCategory>
{
    public void Configure(EntityTypeBuilder<UnitCategory> builder)
    {
        builder.HasKey(ca => ca.Id);

        // Configure Code value object
        builder.OwnsOne(cov => cov.Code, codeBuilder =>
        {
            codeBuilder.WithOwner();

            codeBuilder.Property(code => code.Value)
                .HasColumnName(nameof(UnitCategory.Code))
                .HasConversion(ValueConverters.UpperConverter)
                .HasMaxLength(Code.MaxLength)
                .IsRequired();

            // Add index for title searches
            codeBuilder.HasIndex(code => code.Value)
                .HasDatabaseName("IX_UnitCategories_Code")
                .IsUnique();
        });

        // Configure Description value object
        builder.OwnsOne(pbi => pbi.Description, descBuilder =>
        {
            descBuilder.WithOwner();

            descBuilder.Property(d => d.Value)
                .HasColumnName(nameof(UnitCategory.Description))
                .HasMaxLength(Description.MaxLength)
                .IsRequired(false);
        });

    }
}
