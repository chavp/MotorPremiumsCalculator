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

internal sealed class PolicyTypeConfiguration : IEntityTypeConfiguration<PolicyType>
{
    public void Configure(EntityTypeBuilder<PolicyType> builder)
    {
        builder.HasKey(u => u.Id);

        // Configure Code value object
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
                .HasColumnName(nameof(PolicyType.Name))
                .HasMaxLength(Name.MaxLength);
        });


        // Create index for UnitCategory foreign key
        builder.HasIndex(nameof(PolicyType.Code))
            .HasDatabaseName("IX_PolicyTypes_Code")
            .IsUnique();
    }
}
