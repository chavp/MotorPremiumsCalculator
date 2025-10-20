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

internal class VehicleTypeCompulsoryConfiguration 
    : IEntityTypeConfiguration<VehicleTypeCompulsory>
{
    public void Configure(EntityTypeBuilder<VehicleTypeCompulsory> builder)
    {
        builder.HasKey(e => e.Id);

        // Configure Code value object
        //builder.OwnsOne(e => e.Code, codeBuilder =>
        //{
        //    codeBuilder.WithOwner();

        //    codeBuilder.Property(code => code.Value)
        //        .HasColumnName(nameof(VehicleTypeCompulsory.Code))
        //        .HasConversion(ValueConverters.UpperConverter)
        //        .HasMaxLength(Code.MaxLength)
        //        .IsRequired();

        //    // Add index for title searches
        //    codeBuilder.HasIndex(code => new { code.Value, code.Length })
        //        .HasDatabaseName("IX_VehicleTypeCompulsories_Code")
        //        .IsUnique();
        //});

        builder.Property(e => e.Code)
            .HasConversion(
                e => e.Value.ToUpper(),
                v => Code.Create(v.ToUpper())
            )
            .HasMaxLength(Code.MaxLength)
            .IsRequired();

        //builder
        //    .HasAlternateKey(e => new { e.Code, e.VehicleTypeVoluntary })
        //    .HasName("IX_VehicleTypeCompulsories_Code")
        //    ;

        // Configure Description value object
        builder.OwnsOne(e => e.Description, descBuilder =>
        {
            descBuilder.WithOwner();

            descBuilder.Property(d => d.Value)
                .HasColumnName(nameof(VehicleTypeCompulsory.Description))
                .HasMaxLength(Description.MaxLength)
                .IsRequired(false);
        });

        // Configure relationships
        builder.HasOne(e => e.VehicleTypeVoluntary)
            .WithMany(vehVo => vehVo.VehicleTypeCompulsories)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.HasIndex(e => new { e.VehicleTypeVoluntaryId, e.Code })
            .HasDatabaseName("IX_VehicleTypeCompulsories_VehicleTypeVoluntaryId_Code")
            .IsUnique();
    }
}
