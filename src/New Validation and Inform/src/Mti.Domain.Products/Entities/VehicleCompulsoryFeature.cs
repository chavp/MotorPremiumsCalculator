using EnsureThat;

namespace Mti.Domain.Products.Entities;

public sealed class VehicleCompulsoryFeature : ProductFeature
{
    public Guid? VehicleTypeVoluntaryId { get; private set; }
    public VehicleTypeVoluntary? VehicleTypeVoluntary { get; private set; }

    public Guid? VehicleTypeCompulsoryId { get; private set; }
    public VehicleTypeCompulsory? VehicleTypeCompulsory { get; private set; }

    public decimal Min { get; private set; }
    public decimal Max { get; private set; }

    public Guid? UnitId { get; private set; }
    public Unit? Unit { get; private set; }

    protected VehicleCompulsoryFeature() { }
    public VehicleCompulsoryFeature(
        ProductFeatureType? productFeatureType,
        VehicleTypeVoluntary vehicleTypeVoluntary,
        VehicleTypeCompulsory vehicleTypeCompulsory,
        decimal min, decimal max, Unit unit) : base(productFeatureType) 
    {
        VehicleTypeVoluntary = vehicleTypeVoluntary;
        VehicleTypeCompulsory = vehicleTypeCompulsory;
        Min = min;
        Max = max;
        Unit = unit;
    }

    public VehicleCompulsoryFeature(
        ProductFeatureType? productFeatureType,
        VehicleTypeVoluntary vehicleTypeVoluntary,
        VehicleTypeCompulsory vehicleTypeCompulsory) : base(productFeatureType)
    {
        VehicleTypeVoluntary = vehicleTypeVoluntary;
        VehicleTypeCompulsory = vehicleTypeCompulsory;
    }

    public static Builder CreateBuilder(Product product,
        ProductFeatureAvailabilityType type,
        ProductFeature productFeature)
        => new Builder(product, type, productFeature);

    public sealed class Builder
    {
        internal Guid Id { get; private set; }
        internal Product Product { get; private set; }
        internal ProductFeatureAvailabilityType Type { get; private set; }
        internal ProductFeature ProductFeature { get; private set; }

        internal Builder(Product product,
            ProductFeatureAvailabilityType type,
            ProductFeature productFeature)
        {
            Ensure.That(product).IsNotNull();
            Ensure.That(type).IsNotNull();
            Ensure.That(productFeature).IsNotNull();

            Id = Guid.NewGuid();
            Product = product;
            Type = type;
            ProductFeature = productFeature;
        }

        public VehicleCompulsoryFeature Build()
        {
            var newEntity = new VehicleCompulsoryFeature(this);
            return newEntity;
        }
    }
}
