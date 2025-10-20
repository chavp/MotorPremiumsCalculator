using EnsureThat;
using Mti.Domain.Products.Extensions;
using Mti.Domain.Products.ValueObjects;

namespace Mti.Domain.Products.Entities;

public sealed class VehicleModelFeature : ProductFeature
{
    public Code Code { get; private set; }
    public Name Name { get; private set; }
    public Code MtiCode { get; private set; }

    public Guid? VehicleBrandFeatureId { get; private set; }
    public VehicleBrandFeature? VehicleBrandFeature { get; private set; }

    public Guid? VehiclePriceGroupFeatureId { get; private set; }
    public VehiclePriceGroupFeature? VehiclePriceGroupFeature { get; private set; }

    protected VehicleModelFeature(Builder builder) : base(builder.ProductFeatureType)
    {
        Id = builder.Id;
        Code = builder.Code;
        Name = builder.Name;
        MtiCode = builder.MtiCode;
        VehicleBrandFeature = builder.VehicleBrandFeature;
        VehiclePriceGroupFeature = builder.VehiclePriceGroupFeature;
    }

    protected VehicleModelFeature() : base()
    {
        Code = default!;
    }

    public VehicleModelFeature UpdateCode(string code)
    {
        Code = Code.Create(code);
        return this;
    }
    public VehicleModelFeature UpdateName(string name)
    {
        Name = Name.Create(name);
        return this;
    }
    public VehicleModelFeature UpdateMtiCode(string mtiCode)
    {
        MtiCode = Code.Create(mtiCode);
        return this;
    }

    public static Builder CreateBuilder(string code, string name, 
            ProductFeatureType productFeatureType,
            VehicleBrandFeature vehicleBrandFeature)
        => new Builder(Guid.NewGuid(), Code.Create(code), Name.Create(name),
            vehicleBrandFeature,
            productFeatureType);

    public sealed class Builder
    {
        internal Guid Id { get; } = default!;
        internal Code Code { get; } = default!;
        internal Name Name { get; } = default!;
        internal ProductFeatureType? ProductFeatureType { get; private set; }

        internal Code MtiCode { get; private set; } = default!;
        internal VehicleBrandFeature? VehicleBrandFeature { get; private set;  }
        internal VehiclePriceGroupFeature? VehiclePriceGroupFeature { get; private set; }

        public Builder WithPriceGroup(VehiclePriceGroupFeature? vehiclePriceGroupFeature)
        {
            VehiclePriceGroupFeature = vehiclePriceGroupFeature;
            return this;
        }
        public Builder WithMtiCode(string? mtiCode)
        {
            MtiCode = Code.Create(mtiCode);
            return this;
        }

        internal Builder(Guid id, Code code, Name name,
            VehicleBrandFeature vehicleBrandFeature,
            ProductFeatureType productFeatureType)
        {
            Id = Ensure.That(id).NotEmpty("Id is not empty", nameof(id));
            Code = Ensure.That(code).NotEmpty("Code is not empty", nameof(code));
            Name = Ensure.That(name).NotEmpty("Name is not empty", nameof(name));
            ProductFeatureType = productFeatureType;
            VehicleBrandFeature = vehicleBrandFeature;
        }

        public VehicleModelFeature Build() => new VehicleModelFeature(this);
    }
}
