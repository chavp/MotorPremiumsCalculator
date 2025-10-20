using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mti.Domain.Products.Entities;
using Mti.Domain.Products.ValueObjects;

namespace Mti.Persistence.Configurations;

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
        builder.OwnsOne(pbi => pbi.Name, descBuilder =>
        {
            descBuilder.WithOwner();

            descBuilder.Property(d => d.Value)
                .HasColumnName(nameof(UnitCategory.Name))
                .HasMaxLength(Description.MaxLength)
                .IsRequired(false);
        });

    }
}
