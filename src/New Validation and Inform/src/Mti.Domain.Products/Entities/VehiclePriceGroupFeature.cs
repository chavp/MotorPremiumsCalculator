using EnsureThat;
using Mti.Domain.Products.Extensions;
using Mti.Domain.Products.ValueObjects;

namespace Mti.Domain.Products.Entities;

public sealed class VehiclePriceGroupFeature : ProductFeature
{
    public Code Code { get; private set; }
    public Name Name { get; private set; }

    public decimal Min { get; private set; }
    public decimal Max { get; private set; }

    protected VehiclePriceGroupFeature(Builder builder) : base(builder.ProductFeatureType)
    {
        Id = builder.Id;
        Code = builder.Code;
        Name = builder.Name;
        Min = builder.Min;
        Max = builder.Max;
    }

    protected VehiclePriceGroupFeature() : base()
    {
        Code = default!;
    }

    public VehiclePriceGroupFeature UpdateCode(string code)
    {
        Code = Code.Create(code);
        return this;
    }
    public VehiclePriceGroupFeature UpdateName(string name)
    {
        Name = Name.Create(name);
        return this;
    }

    public VehiclePriceGroupFeature UpdateRange(decimal min, decimal max)
    {
        Min = Math.Min(min, max);
        Max = Math.Max(max, min);
        return this;
    }

    public static Builder CreateBuilder(string code, string name, ProductFeatureType productFeatureType)
        => new Builder(Guid.NewGuid(), Code.Create(code), Name.Create(name),
            productFeatureType);

    public sealed class Builder
    {
        internal Guid Id { get; private set; } = default!;
        internal Code Code { get; private set; } = default!;
        internal Name Name { get; private set; } = default!;
        internal ProductFeatureType? ProductFeatureType { get; private set; }

        internal decimal Min { get; private set; }
        internal decimal Max { get; private set; }

        public Builder WithRange(decimal min, decimal max)
        {
            Min = Math.Min(min, max);
            Max = Math.Max(max, min);
            return this;
        }

        internal Builder(Guid id, Code code, Name name, ProductFeatureType productFeatureType)
        {
            Id = Ensure.That(id).NotEmpty("Id is not empty", nameof(id));
            Code = Ensure.That(code).NotEmpty("Code is not empty", nameof(code));
            Name = Ensure.That(name).NotEmpty("Name is not empty", nameof(name));
            ProductFeatureType = productFeatureType;
        }

        public VehiclePriceGroupFeature Build() => new VehiclePriceGroupFeature(this);
    }
}
