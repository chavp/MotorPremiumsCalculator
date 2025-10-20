using Mti.Domain.Products.ValueObjects;
using Mti.Domain.SharedKernel.Primatives;

namespace Mti.Domain.Products.Entities;

public abstract class ProductFeature : Entity<Guid>
{
    public Guid? ProductFeatureTypeId { get; private set; }
    public ProductFeatureType? ProductFeatureType { get; private set; }

    protected ProductFeature() { }
    protected ProductFeature(ProductFeatureType? productFeatureType) => ProductFeatureType = productFeatureType;
}
