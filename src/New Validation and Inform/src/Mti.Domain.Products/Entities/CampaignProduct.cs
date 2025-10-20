using EnsureThat;
using Mti.Domain.Products.Extensions;
using Mti.Domain.SharedKernel.Primatives;

namespace Mti.Domain.Products.Entities;

public sealed class CampaignProduct : Entity<Guid>
{
    public Guid? ProductId { get; private set; }
    public Product? Product { get; private set; }

    public Guid? CampaignId { get; private set; }
    public Campaign? Campaign { get; private set; }

    protected CampaignProduct() { }

    public CampaignProduct(
        Guid id,
        Product product,
        Campaign campaign) : base(id)
    {
        Ensure.That(product).IsNotNull();
        Ensure.That(campaign).IsNotNull();
        Id = Ensure.That(id).NotEmpty("Id is not empty", nameof(id));
        Product = product;
        Campaign = campaign;
    }
}
