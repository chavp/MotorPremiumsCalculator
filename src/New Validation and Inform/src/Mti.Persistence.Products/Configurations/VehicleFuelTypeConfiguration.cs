using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mti.Domain.Products.Entities;
using Mti.Domain.Products.ValueObjects;

namespace Mti.Persistence.Products.Configurations;

internal sealed class VehicleFuelTypeConfiguration
    : IEntityTypeConfiguration<VehicleFuelType>
{
    public void Configure(EntityTypeBuilder<VehicleFuelType> builder)
    {
        builder.HasKey(e => e.Id);

        // Configure Code value object
        //builder.OwnsOne(e => e.Code, codeBuilder =>
        //{
        //    codeBuilder.WithOwner();

        //    codeBuilder.Property(code => code.Value)
        //        .HasColumnName(nameof(VehicleFuelType.Code))
        //        .HasConversion(ValueConverters.UpperConverter)
        //        .HasMaxLength(Code.MaxLength)
        //        .IsRequired();

        //    // Add index for title searches
        //    codeBuilder.HasIndex(code => code.Value)
        //        .HasDatabaseName("IX_VehicleFuelTypes_Code")
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
        builder.OwnsOne(e => e.Name, descBuilder =>
        {
            descBuilder.WithOwner();

            descBuilder.Property(d => d.Value)
                .HasColumnName(nameof(VehicleFuelType.Name))
                .HasMaxLength(Name.MaxLength)
                .IsRequired(false);
        });

        // Configure Prefix value object
        builder.OwnsOne(e => e.Prefix, descBuilder =>
        {
            descBuilder.WithOwner();

            descBuilder.Property(d => d.Value)
                .HasColumnName(nameof(VehicleFuelType.Prefix))
                .HasMaxLength(Prefix.MaxLength)
                .IsRequired(false);
        });

        // Create index for VehicleFuelType foreign key
        builder.HasIndex(nameof(VehicleFuelType.Code))
            .HasDatabaseName("IX_VehicleFuelTypes_Code")
            .IsUnique();
    }
}
