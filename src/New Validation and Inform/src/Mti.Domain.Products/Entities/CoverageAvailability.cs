using EnsureThat;
using Mti.Domain.Products.Extensions;
using Mti.Domain.Products.ValueObjects;
using Mti.Domain.SharedKernel.Primatives;

namespace Mti.Domain.Products.Entities;

public sealed class CoverageAvailability : Entity<Guid>
{
    public Guid? ProductId { get; private set; }
    public Product? Product { get; private set; }

    public Guid? CoverageAvailabilityTypeId { get; private set; }
    public CoverageAvailabilityType? CoverageAvailabilityType { get; private set; }

    public Guid? CoverageTypeId { get; private set; }
    public CoverageType? CoverageType { get; private set; }

    public Guid? CoverageLevelId { get; private set; }
    public CoverageLevel? CoverageLevel { get; private set; }

    protected CoverageAvailability() { }

    protected CoverageAvailability(Builder builder) : base(builder.Id)
    {
        Product = builder.Product;
        CoverageAvailabilityType = builder.Type;
        CoverageType = builder.CoverageType;
        CoverageLevel = builder.CoverageLevel;
    }

    public static Builder CreateBuilder(Product product,
        CoverageAvailabilityType type,
        CoverageType coverageType,
        CoverageLevel coverageLevel)
        => new Builder(product, type, coverageType, coverageLevel);

    public sealed class Builder
    {
        internal Guid Id { get; private set; }
        internal Product Product { get; private set; }
        internal CoverageAvailabilityType Type { get; private set; } 
        internal CoverageType CoverageType { get; private set; } 
        internal CoverageLevel CoverageLevel { get; private set; } 

        internal Builder(Product product,
            CoverageAvailabilityType type,
            CoverageType coverageType,
            CoverageLevel coverageLevel)
        {
            Ensure.That(product).IsNotNull();
            Ensure.That(type).IsNotNull();
            Ensure.That(coverageType).IsNotNull();
            Ensure.That(coverageLevel).IsNotNull();

            Id = Guid.NewGuid();
            Product = product;
            Type = type;
            CoverageType = coverageType;
            CoverageLevel = coverageLevel;
        }

        public CoverageAvailability Build()
        {
            var newEntity = new CoverageAvailability(this);
            return newEntity;
        }
    }
}
