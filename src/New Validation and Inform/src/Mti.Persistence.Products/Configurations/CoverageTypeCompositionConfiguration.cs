using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mti.Domain.Products.Entities;

namespace Mti.Persistence.Configurations;

internal sealed class CoverageTypeCompositionConfiguration 
    : IEntityTypeConfiguration<CoverageTypeComposition>
{
    public void Configure(EntityTypeBuilder<CoverageTypeComposition> builder)
    {
        builder.HasKey(e => e.Id);

        // Configure relationships
        builder.HasOne(e => e.FromCoverageType)
            .WithMany(to => to.ToCompositions)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.HasOne(e => e.ToCoverageType)
            .WithMany(from => from.FromCompositions)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.HasIndex(e => new { e.FromCoverageTypeId, e.ToCoverageTypeId })
        .HasDatabaseName("IX_CoverageTypeCompositions_FromToCoverageTypeId")
        .IsUnique();
    }
}
