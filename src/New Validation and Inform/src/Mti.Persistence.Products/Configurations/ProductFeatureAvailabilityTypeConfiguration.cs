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

internal sealed class ProductFeatureAvailabilityTypeConfiguration 
    : IEntityTypeConfiguration<ProductFeatureAvailabilityType>
{
    public void Configure(EntityTypeBuilder<ProductFeatureAvailabilityType> builder)
    {
        builder.HasKey(cov => cov.Id);

        // Configure Code value object
        builder.Property(e => e.Code)
        .HasConversion(
            e => e.Value.ToUpperInvariant(),
            v => Code.Create(v.ToUpperInvariant())
        )
        .HasMaxLength(Code.MaxLength)
        .IsRequired();

        // Configure Description value object
        builder.OwnsOne(pbi => pbi.Description, descBuilder =>
        {
            descBuilder.WithOwner();

            descBuilder.Property(d => d.Value)
                .HasColumnName(nameof(ProductFeatureAvailabilityType.Description))
                .HasMaxLength(Description.MaxLength)
                .IsRequired(false);
        });


        // Add index for title searches
        builder.HasIndex(nameof(ProductFeatureAvailabilityType.Code))
            .HasDatabaseName("IX_ProductFeatureAvailabilityTypes_Code")
            .IsUnique();
    }
}
