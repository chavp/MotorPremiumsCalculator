using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mti.Domain.Products.Entities;
using Mti.Domain.Products.ValueObjects;
using Mti.Persistence.Configurations;

namespace Mti.Persistence.Products.Configurations;

public sealed class ProductFeatureTypeConfiguration
: IEntityTypeConfiguration<ProductFeatureType>
{
    public void Configure(EntityTypeBuilder<ProductFeatureType> builder)
    {
        builder.HasKey(cov => cov.Id);

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
                .HasColumnName(nameof(ProductFeatureType.Description))
                .HasMaxLength(Description.MaxLength)
                .IsRequired(false);
        });

        // Add index for title searches
        builder.HasIndex(nameof(ProductFeatureType.Code))
            .HasDatabaseName("IX_ProductFeatureTypes_Code")
            .IsUnique();
    }
}
