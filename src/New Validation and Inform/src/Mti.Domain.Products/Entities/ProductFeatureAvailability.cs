using EnsureThat;
using Mti.Domain.Products.Extensions;
using Mti.Domain.SharedKernel.Primatives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mti.Domain.Products.Entities;

public sealed class ProductFeatureAvailability : Entity<Guid>
{
    public Guid? ProductId { get; private set; }
    public Product? Product { get; private set; }

    public Guid? ProductFeatureAvailabilityTypeId { get; private set; }
    public ProductFeatureAvailabilityType? ProductFeatureAvailabilityType { get; private set; }

    public Guid? ProductFeatureId { get; private set; }
    public ProductFeature? ProductFeature { get; private set; }

    protected ProductFeatureAvailability() { }

    protected ProductFeatureAvailability(Builder builder) : base(builder.Id)
    {
        Product = builder.Product;
        ProductFeatureAvailabilityType = builder.Type;
        ProductFeature = builder.ProductFeature;
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

        public ProductFeatureAvailability Build()
        {
            var newEntity = new ProductFeatureAvailability(this);
            return newEntity;
        }
    }
}
