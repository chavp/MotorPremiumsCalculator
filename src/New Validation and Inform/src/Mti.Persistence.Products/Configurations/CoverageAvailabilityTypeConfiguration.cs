using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mti.Domain.Products.Entities;
using Mti.Domain.Products.ValueObjects;

namespace Mti.Persistence.Configurations;

internal sealed class CoverageAvailabilityTypeConfiguration 
    : IEntityTypeConfiguration<CoverageAvailabilityType>
{
    public void Configure(EntityTypeBuilder<CoverageAvailabilityType> builder)
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
                .HasColumnName(nameof(CoverageAvailabilityType.Description))
                .HasMaxLength(Description.MaxLength)
                .IsRequired(false);
        });


        // Add index for title searches
        builder.HasIndex(nameof(CoverageAvailabilityType.Code))
            .HasDatabaseName("IX_CoverageAvailabilityTypes_Code")
            .IsUnique();
    }
}
