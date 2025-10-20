using EnsureThat;
using Mti.Domain.Products.Extensions;
using Mti.Domain.Products.ValueObjects;

namespace Mti.Domain.Products.Entities;

public sealed class VehicleBrandFeature : ProductFeature
{
    public Code Code { get; private set; }
    public Name Name { get; private set; }

    public List<VehicleModelFeature> Models { get; private set; } = [];

    protected VehicleBrandFeature(Builder builder) : base(builder.ProductFeatureType)
    {
        Id = builder.Id;
        Code = builder.Code;
        Name = builder.Name;
    }

    protected VehicleBrandFeature() : base()
    {
        Code = default!;
    }

    public VehicleBrandFeature UpdateCode(string code)
    {
        Code = Code.Create(code);
        return this;
    }
    public VehicleBrandFeature UpdateName(string name)
    {
        Name = Name.Create(name);
        return this;
    }

    public static Builder CreateBuilder(string code, string name, ProductFeatureType productFeatureType) 
        => new Builder(Code.Create(code), Name.Create(name), 
            productFeatureType);

    public sealed class Builder
    {
        internal Guid Id { get; private set; } = default!;
        internal Code Code { get; private set; } = default!;
        internal Name Name { get; private set; } = default!;
        internal ProductFeatureType? ProductFeatureType { get; private set; }

        internal Builder(Code code, Name name, ProductFeatureType productFeatureType)
        {
            Ensure.That(productFeatureType, nameof(productFeatureType)).IsNotNull();
            Code = Ensure.That(code).NotEmpty("Code is not empty", nameof(code));
            Name = Ensure.That(name).NotEmpty("Name is not empty", nameof(name));
            Id = Guid.NewGuid();
            ProductFeatureType = productFeatureType;
        }

        public VehicleBrandFeature Build() => new VehicleBrandFeature(this);
    }
}
