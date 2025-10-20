namespace Mti.Domain.Products.Entities;

public sealed class VehicleVoluntaryFeature : ProductFeature
{

    public Guid? VehicleWorkshopTypeId { get; set; }
    public VehicleWorkshopType? VehicleWorkshopType { get; set; }

    public Guid? VehicleTypeVoluntaryId { get; set; }
    public VehicleTypeVoluntary? VehicleTypeVoluntary { get; set; }

    protected VehicleVoluntaryFeature() { }
    public VehicleVoluntaryFeature(ProductFeatureType? productFeatureType): base(productFeatureType) { }

}
