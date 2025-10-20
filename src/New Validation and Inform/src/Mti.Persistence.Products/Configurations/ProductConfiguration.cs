using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mti.Domain.Products.Entities;
using Mti.Domain.Products.ValueObjects;

namespace Mti.Persistence.Configurations;

internal sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(cov => cov.Id);

        //// Configure Code value object
        //builder.OwnsOne(cov => cov.Code, codeBuilder =>
        //{
        //    codeBuilder.WithOwner();

        //    codeBuilder.Property(code => code.Value)
        //        .HasColumnName(nameof(Product.Code))
        //        .HasConversion(ValueConverters.UpperConverter)
        //        .HasMaxLength(Code.MaxLength)
        //        .IsRequired();

        //    // Add index for title searches
        //    codeBuilder.HasIndex(code => code.Value)
        //        .HasDatabaseName("IX_Products_Code")
        //        .IsUnique();
        //});

        //// Configure Name value object
        //builder.OwnsOne(pbi => pbi.Name, descBuilder =>
        //{
        //    descBuilder.WithOwner();

        //    descBuilder.Property(d => d.Value)
        //        .HasColumnName(nameof(Product.Name))
        //        .HasMaxLength(Name.MaxLength)
        //        .IsRequired(false);
        //});

        // Configure Code value object
        builder.Property(e => e.Code)
        .HasConversion(
            e => e.Value.ToUpperInvariant(),
            v => Code.Create(v.ToUpperInvariant())
        )
        .HasMaxLength(Code.MaxLength)
        .IsRequired();

        // Configure Name value object
        builder.Property(e => e.Name)
        .HasConversion(
            e => e.Value.ToUpperInvariant(),
            v => Name.Create(v.ToUpperInvariant())
        )
        .HasMaxLength(Name.MaxLength)
        .IsRequired();

        // Configure Description value object
        builder.OwnsOne(pbi => pbi.Description, descBuilder =>
        {
            descBuilder.WithOwner();

            descBuilder.Property(d => d.Value)
                .HasColumnName(nameof(Product.Description))
                .HasMaxLength(Description.MaxLength)
                .IsRequired(false);
        });

        builder.HasIndex(nameof(Product.Code))
            .HasDatabaseName("IX_Products_Code")
            .IsUnique();
    }
}
