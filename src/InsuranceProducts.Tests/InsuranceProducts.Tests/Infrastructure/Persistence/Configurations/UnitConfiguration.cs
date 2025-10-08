using InsuranceProducts.Tests.Domain.Products.Entities;
using InsuranceProducts.Tests.Domain.Products.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceProducts.Tests.Infrastructure.Persistence.Configurations
{
    internal sealed class UnitConfiguration : IEntityTypeConfiguration<Unit>
    {
        public void Configure(EntityTypeBuilder<Unit> builder)
        {
            builder.HasKey(u => u.Id);

            // Configure Code value object
            builder.OwnsOne(u => u.Code, codeBuilder =>
            {
                //codeBuilder.WithOwner();

                codeBuilder.Property(code => code.Value)
                    .HasColumnName(nameof(Unit.Code))
                    .HasConversion(ValueConverters.UpperConverter)
                    .HasMaxLength(Code.MaxLength)
                    .IsRequired();

                // Create index for Code value inside the owned entity configuration
                //codeBuilder.HasIndex(code => code.Value)
                //    .HasDatabaseName("IX_Units_Code");
            });

            // Configure Description value object
            builder.OwnsOne(e => e.Description, descBuilder =>
            {
                descBuilder.WithOwner();

                descBuilder.Property(v => v.Value)
                    .HasColumnName(nameof(Unit.Description))
                    .HasMaxLength(Description.MaxLength)
                    .IsRequired(false);
            });

            builder.OwnsOne(e => e.Symbol, descBuilder =>
            {
                descBuilder.WithOwner();

                descBuilder.Property(v => v.Value)
                    .HasColumnName(nameof(Unit.Symbol))
                    .HasMaxLength(Symbol.MaxLength)
                    .IsRequired(false);
            });

            // Configure relationships
            builder.HasOne(ca => ca.UnitCategory)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            // Create index for UnitCategory foreign key
            //builder.HasIndex(nameof(Unit.UnitCategoryId), nameof(Unit.Code))
            //    .HasDatabaseName("IX_Units_UnitCategoryId_Code");

        }
    }
}
