using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mti.Domain.Products.Entities;
using Mti.Domain.Products.ValueObjects;

namespace Mti.Persistence.Configurations;

internal sealed class CoverageLevelTypeConfiguration 
    : IEntityTypeConfiguration<CoverageLevelType>
{
    public void Configure(EntityTypeBuilder<CoverageLevelType> builder)
    {
        builder.HasKey(cov => cov.Id);

        // Configure Code value object
        //builder.OwnsOne(cov => cov.Code, codeBuilder =>
        //{
        //    codeBuilder.WithOwner();

        //    codeBuilder.Property(code => code.Value)
        //        .HasColumnName(nameof(CoverageType.Code))
        //        .HasConversion(ValueConverters.UpperConverter)
        //        .HasMaxLength(Code.MaxLength)
        //        .IsRequired();

        //    // Add index for title searches
        //    codeBuilder.HasIndex(code => code.Value)
        //        .HasDatabaseName("IX_CoverageLevelTypes_Code")
        //        .IsUnique();
        //});
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
                .HasColumnName(nameof(CoverageLevelType.Description))
                .HasMaxLength(Description.MaxLength)
                .IsRequired(false);
        });

        builder.HasIndex(nameof(CoverageLevelType.Code))
            .HasDatabaseName("IX_CoverageLevelTypes_Code")
            .IsUnique();
    }
}
