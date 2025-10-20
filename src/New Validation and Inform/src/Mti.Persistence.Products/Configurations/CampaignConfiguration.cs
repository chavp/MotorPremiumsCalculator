using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mti.Domain.Products.Entities;
using Mti.Domain.Products.ValueObjects;

namespace Mti.Persistence.Products.Configurations;

internal sealed class CampaignConfiguration : IEntityTypeConfiguration<Campaign>
{
    public void Configure(EntityTypeBuilder<Campaign> builder)
    {
        builder.HasKey(cov => cov.Id);

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
                .HasColumnName(nameof(Campaign.Description))
                .HasMaxLength(Description.MaxLength)
                .IsRequired(false);
        });

        builder.HasOne(e => e.PolicyType)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.HasIndex(nameof(Campaign.PolicyTypeId), nameof(Campaign.Code))
            .HasDatabaseName("IX_Campaigns_PolicyTypeId_Code")
            .IsUnique()
            ;
    }
}
