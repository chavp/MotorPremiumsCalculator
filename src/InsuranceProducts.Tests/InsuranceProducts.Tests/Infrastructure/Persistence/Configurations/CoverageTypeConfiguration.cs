using InsuranceProducts.Tests.Domain.Products.Entities;
using InsuranceProducts.Tests.Domain.Products.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceProducts.Tests.Infrastructure.Persistence.Configurations
{
    public class CoverageTypeConfiguration : IEntityTypeConfiguration<CoverageType>
    {
        public void Configure(EntityTypeBuilder<CoverageType> builder)
        {
            builder.HasKey(cov => cov.Id);

            // Configure Id as value object
            //builder.Property(cov => cov.Id)
            //    .HasConversion(
            //        id => id.Value,
            //        value => CoverageTypeId.From(value))
            //    .ValueGeneratedNever();

            // Configure Code value object
            builder.OwnsOne(cov => cov.Code, codeBuilder =>
            {
                codeBuilder.WithOwner();

                codeBuilder.Property(code => code.Value)
                    .HasColumnName(nameof(CoverageType.Code))
                    .HasConversion(ValueConverters.UpperConverter)
                    .HasMaxLength(Code.MaxLength)
                    .IsRequired();

                // Add index for title searches
                codeBuilder.HasIndex(code => code.Value)
                    .HasDatabaseName("IX_CoverageTypes_Code")
                    .IsUnique();
            });

            // Configure Description value object
            builder.OwnsOne(pbi => pbi.Description, descBuilder =>
            {
                descBuilder.WithOwner();

                descBuilder.Property(d => d.Value)
                    .HasColumnName(nameof(CoverageType.Description))
                    .HasMaxLength(Description.MaxLength)
                    .IsRequired(false);
            });
        }
    }
}
