using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mti.Domain.Products.Entities;
using Mti.Domain.Products.ValueObjects;

namespace Mti.Persistence.Products.Configurations;

internal sealed class VehicleWorkshopTypeConfiguration
: IEntityTypeConfiguration<VehicleWorkshopType>
{
    public void Configure(EntityTypeBuilder<VehicleWorkshopType> builder)
    {
        builder.HasKey(e => e.Id);

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

        builder.Property(e => e.LookupNames)
            .HasMaxLength(Name.MaxLength * 10)
            .IsRequired(false);

        // Create index for VehicleFuelType foreign key
        builder.HasIndex(nameof(VehicleWorkshopType.Code))
            .HasDatabaseName("IX_VehicleWorkshopTypes_Code")
            .IsUnique();

        builder.HasIndex(nameof(VehicleWorkshopType.LookupNames))
            .HasDatabaseName("IX_VehicleWorkshopTypes_LookupNames");
    }
}
