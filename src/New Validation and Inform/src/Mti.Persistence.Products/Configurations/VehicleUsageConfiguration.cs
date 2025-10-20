using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mti.Domain.Products.Entities;
using Mti.Domain.Products.ValueObjects;

namespace Mti.Persistence.Configurations;

internal sealed class VehicleUsageConfiguration
    : IEntityTypeConfiguration<VehicleUsage>
{
    public void Configure(EntityTypeBuilder<VehicleUsage> builder)
    {
        builder.HasKey(e => e.Id);

        // Configure Code value object
        builder.OwnsOne(cov => cov.Code, codeBuilder =>
        {
            codeBuilder.WithOwner();

            codeBuilder.Property(code => code.Value)
                .HasColumnName(nameof(VehicleUsage.Code))
                .HasConversion(ValueConverters.UpperConverter)
                .HasMaxLength(Code.MaxLength)
                .IsRequired();

            // Add index for title searches
            codeBuilder.HasIndex(code => code.Value)
                .HasDatabaseName("IX_VehicleUsages_Code")
                .IsUnique();
        });

        // Configure Name value object
        builder.OwnsOne(e => e.Name, descBuilder =>
        {
            descBuilder.WithOwner();

            descBuilder.Property(d => d.Value)
                .HasColumnName(nameof(VehicleUsage.Name))
                .HasMaxLength(Name.MaxLength)
                .IsRequired(false);
        });
    }
}
