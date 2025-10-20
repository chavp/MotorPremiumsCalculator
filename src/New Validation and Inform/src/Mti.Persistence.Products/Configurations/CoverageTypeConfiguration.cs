using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mti.Domain.Products.Entities;
using Mti.Domain.Products.ValueObjects;

namespace Mti.Persistence.Configurations;

internal sealed class CoverageTypeConfiguration : IEntityTypeConfiguration<CoverageType>
{
    public void Configure(EntityTypeBuilder<CoverageType> builder)
    {
        builder.HasKey(cov => cov.Id);

        // Configure Id as value object
        //builder.Property(cov => cov.Id)
        //    .HasConversion(
        //        id => id.Value,
        //        value => CoverageTypeId.From(value))
        //    .ValueGeneratedNever();

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
        //        .HasDatabaseName("IX_CoverageTypes_Code")
        //        .IsUnique();
        //});

        builder.Property(e => e.Code)
        .HasConversion(
            e => e.Value.ToUpperInvariant(),
            v => Code.Create(v.ToUpperInvariant())
        )
        .HasMaxLength(Code.MaxLength)
        .IsRequired();

        // Configure Name value object
        builder.OwnsOne(pbi => pbi.Name, descBuilder =>
        {
            descBuilder.WithOwner();

            descBuilder.Property(d => d.Value)
                .HasColumnName(nameof(CoverageType.Name))
                .HasMaxLength(Name.MaxLength)
                .IsRequired(false);
        });

        // Configure Description value object
        builder.OwnsOne(pbi => pbi.Description, descBuilder =>
        {
            descBuilder.WithOwner();

            descBuilder.Property(d => d.Value)
                .HasColumnName(nameof(CoverageType.Description))
                .HasMaxLength(Description.MaxLength)
                .IsRequired(false);
        });

        builder.HasIndex(nameof(CoverageType.Code))
            .HasDatabaseName("IX_CoverageTypes_Code")
            .IsUnique();
    }
}
