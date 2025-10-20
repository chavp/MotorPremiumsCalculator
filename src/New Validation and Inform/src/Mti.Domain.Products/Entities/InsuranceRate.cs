using EnsureThat;
using Mti.Domain.Products.Extensions;
using Mti.Domain.Products.ValueObjects;
using Mti.Domain.SharedKernel.Primatives;

namespace Mti.Domain.Products.Entities;

public sealed class InsuranceRate : Entity<Guid>
{
    public Guid? ProductId { get; private set; }
    public Product? Product { get; private set; }

    public Guid? CoverageTypeId { get; private set; }
    public CoverageType? CoverageType { get; private set; }

    public Guid? CoverageLevelId { get; private set; }
    public CoverageLevel? CoverageLevel { get; private set; }

    public Guid? ProductFeatureId { get; private set; }
    public ProductFeature? ProductFeature { get; private set; }


    public Guid? PeriodTypeId { get; private set; }
    public PeriodType? PeriodType { get; private set; }

    public Guid? UnitId { get; private set; }
    public Unit? Unit { get; private set; }


    public decimal RateAmount { get; private set; }
    public DateOnly EffectiveDate { get; private set; } = DateOnly.FromDateTime(DateTime.Now);
    public DateOnly ExpiryDate { get; private set; } = DateOnly.FromDateTime(DateTime.MaxValue);

    public InsuranceRate UpdateRateAmount(decimal rateAmount)
    {
        RateAmount = rateAmount;
        return this;
    }

    protected InsuranceRate() { }
    protected InsuranceRate(Builder builder) : base(builder.Id)
    {
        Product = builder.Product;
        ProductFeature = builder.ProductFeature;
        CoverageType = builder.CoverageType;
        CoverageLevel = builder.CoverageLevel;
        RateAmount = builder.RateAmount;
        Unit = builder.Unit;
        PeriodType = builder.PeriodType;
        EffectiveDate = builder.EffectiveDate;
        ExpiryDate = builder.ExpiryDate;
    }

    public static Builder CreateBuilder(Product product, Unit unit, PeriodType periodType) 
        => new Builder(product, unit, periodType);

    public sealed class Builder
    {
        internal Guid Id { get; private set; }
        internal Product? Product { get; private set; }
        internal CoverageType? CoverageType { get; private set; }
        internal CoverageLevel? CoverageLevel { get; private set; }
        internal ProductFeature? ProductFeature { get; private set; }
        internal PeriodType? PeriodType { get; private set; }
        internal Unit? Unit { get; private set; }


        internal decimal RateAmount { get; private set; }
        internal DateOnly EffectiveDate { get; private set; } = DateOnly.FromDateTime(DateTime.Now);
        internal DateOnly ExpiryDate { get; private set; } = DateOnly.FromDateTime(DateTime.MaxValue);

        public Builder WithCoverageType(CoverageType coverageType)
        {
            CoverageType = coverageType;
            return this;
        }
        public Builder WithCoverageLevel(CoverageLevel coverageLevel)
        {
            CoverageLevel = coverageLevel;
            return this;
        }
        public Builder WithProductFeature(ProductFeature productFeature)
        {
            ProductFeature = productFeature;
            return this;
        }
        public Builder WithRateAmount(decimal rateAmount)
        {
            RateAmount = rateAmount;
            return this;
        }
        public Builder WithEffectiveDate(DateOnly effectiveDate)
        {
            EffectiveDate = effectiveDate;
            return this;
        }
        public Builder WithExpiryDate(DateOnly expiryDate)
        {
            ExpiryDate = expiryDate;
            return this;
        }

        internal Builder(Product product, Unit unit, PeriodType periodType)
        {
            Id = Guid.NewGuid();

            Ensure.That(product, nameof(product)).IsNotNull();
            Ensure.That(unit, nameof(unit)).IsNotNull();
            Ensure.That(periodType, nameof(periodType)).IsNotNull();

            Product = product;
            Unit = unit;
            PeriodType = periodType;
        }

        public InsuranceRate Build()
        {
            var newEntity = new InsuranceRate(this);
            return newEntity;
        }
    }
}
