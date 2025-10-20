using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mti.Domain.Products.Entities;
using Mti.Domain.Products.ValueObjects;
using Mti.Persistence.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mti.Persistence.Products.Configurations;

internal sealed class VehicleTypeVoluntaryConfiguration
    : IEntityTypeConfiguration<VehicleTypeVoluntary>
{
    public void Configure(EntityTypeBuilder<VehicleTypeVoluntary> builder)
    {
        builder.HasKey(e => e.Id);

        // Configure relationships
        builder.HasOne(e => e.VehicleFuelType)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.HasOne(e => e.VehicleType)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.HasOne(e => e.VehicleUsage)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        // Configure Code value object
        builder.Property(e => e.Code)
        .HasConversion(
            e => e.Value.ToUpperInvariant(),
            v => Code.Create(v.ToUpperInvariant())
        )
        .HasMaxLength(Code.MaxLength)
        .IsRequired();

        // Configure Name value object
        builder.OwnsOne(e => e.Name, eBuilder =>
        {
            eBuilder.WithOwner();

            eBuilder.Property(d => d.Value)
                .HasColumnName(nameof(VehicleTypeVoluntary.Name))
                .HasMaxLength(Name.MaxLength)
                .IsRequired(false);
        });

        // Configure Description value object
        builder.OwnsOne(e => e.Description, eBuilder =>
        {
            eBuilder.WithOwner();

            eBuilder.Property(d => d.Value)
                .HasColumnName(nameof(VehicleTypeVoluntary.Description))
                .HasMaxLength(Description.MaxLength)
                .IsRequired(false);
        });

        // Configure indexes
        builder.HasIndex(e => new { e.VehicleFuelTypeId, e.VehicleTypeId, e.VehicleUsageId })
        .HasDatabaseName("IX_VehicleTypeVoluntaries_VehicleFuelTypeUsages")
        .IsUnique();

        // Add index for title searches
        builder.HasIndex(e => e.Code)
            .HasDatabaseName("IX_VehicleTypeVoluntaries_Code")
            .IsUnique();
    }
}
