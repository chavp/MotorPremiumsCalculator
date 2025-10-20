using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mti.Domain.Products.Entities;
using Mti.Domain.Products.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mti.Persistence.Products.Configurations;

internal sealed class PeriodTypeConfiguration : IEntityTypeConfiguration<PeriodType>
{
    public void Configure(EntityTypeBuilder<PeriodType> builder)
    {
        builder.HasKey(e => e.Id);

        // Configure Code value object
        builder.Property(e => e.Code)
        .HasConversion(
            e => e.Value.ToUpperInvariant(),
            v => Code.Create(v.ToUpperInvariant())
        )
        .HasMaxLength(Code.MaxLength)
        .IsRequired();

        builder.Property(e => e.Name)
         .HasConversion(
             e => e.Value,
             v => Name.Create(v)
         )
         .HasMaxLength(Name.MaxLength)
         .IsRequired();

        // Add index for PeriodType searches
        builder.HasIndex(nameof(PeriodType.Code))
            .HasDatabaseName("IX_PeriodTypes_Code")
            .IsUnique();
    }
}
