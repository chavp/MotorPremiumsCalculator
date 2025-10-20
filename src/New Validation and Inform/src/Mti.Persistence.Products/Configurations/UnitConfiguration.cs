using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mti.Domain.Products.Entities;
using Mti.Domain.Products.ValueObjects;

namespace Mti.Persistence.Configurations;

internal sealed class UnitConfiguration : IEntityTypeConfiguration<Unit>
{
    public void Configure(EntityTypeBuilder<Unit> builder)
    {
        builder.HasKey(u => u.Id);

        // Configure Code value object
        //builder.OwnsOne(u => u.Code, codeBuilder =>
        //{
        //    //codeBuilder.WithOwner();

        //    codeBuilder.Property(code => code.Value)
        //        .HasColumnName(nameof(Unit.Code))
        //        .HasConversion(ValueConverters.UpperConverter)
        //        .HasMaxLength(Code.MaxLength)
        //        .IsRequired();

        //    // Create index for Code value inside the owned entity configuration
        //    //codeBuilder.HasIndex(code => code.Value)
        //    //    .HasDatabaseName("IX_Units_Code");
        //});

        builder.Property(e => e.Code)
        .HasConversion(
            e => e.Value.ToUpperInvariant(),
            v => Code.Create(v.ToUpperInvariant())
        )
        .HasMaxLength(Code.MaxLength)
        .IsRequired();

        // Configure Name value object
        builder.OwnsOne(e => e.Name, descBuilder =>
        {
            descBuilder.WithOwner();

            descBuilder.Property(v => v.Value)
                .HasColumnName(nameof(Unit.Name))
                .HasMaxLength(Name.MaxLength)
                .IsRequired(false);
        });

        builder.OwnsOne(e => e.Symbol, descBuilder =>
        {
            descBuilder.WithOwner();

            descBuilder.Property(v => v.Value)
                .HasColumnName(nameof(Unit.Symbol))
                .HasMaxLength(Symbol.MaxLength)
                .IsRequired(false);
        });

        // Configure relationships
        builder.HasOne(ca => ca.UnitCategory)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        // Create index for UnitCategory foreign key
        builder.HasIndex(nameof(Unit.UnitCategoryId), nameof(Unit.Code))
            .HasDatabaseName("IX_Units_UnitCategoryId_Code")
            .IsUnique();
    }
}
