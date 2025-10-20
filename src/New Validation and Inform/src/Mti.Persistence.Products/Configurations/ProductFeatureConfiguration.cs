using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mti.Domain.Products.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mti.Persistence.Products.Configurations;

internal sealed class ProductFeatureConfiguration 
    : IEntityTypeConfiguration<ProductFeature>
{
    public void Configure(EntityTypeBuilder<ProductFeature> builder)
    {
        builder.HasKey(pf => pf.Id);

        // Configure relationships
        builder.HasOne(pf => pf.ProductFeatureType)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

    }
}
