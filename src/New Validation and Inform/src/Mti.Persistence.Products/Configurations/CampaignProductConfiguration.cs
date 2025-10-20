using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mti.Domain.Products.Entities;

namespace Mti.Persistence.Products.Configurations;

internal sealed class CampaignProductConfiguration
: IEntityTypeConfiguration<CampaignProduct>
{
    public void Configure(EntityTypeBuilder<CampaignProduct> builder)
    {
        builder.HasKey(ca => ca.Id);

        // Configure relationships
        builder.HasOne(cp => cp.Product)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.HasOne(cp => cp.Campaign)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        // Configure indexes
        builder.HasIndex(cp => new { cp.ProductId, cp.CampaignId })
        .HasDatabaseName("IX_CampaignProducts_ProductId_CampaignId")
        .IsUnique();

    }
}
