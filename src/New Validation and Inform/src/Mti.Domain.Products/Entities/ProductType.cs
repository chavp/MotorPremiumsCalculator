using EnsureThat;
using Mti.Domain.Products.Extensions;
using Mti.Domain.Products.ValueObjects;
using Mti.Domain.SharedKernel.Primatives;

namespace Mti.Domain.Products.Entities;

public sealed class ProductType : Entity<Guid>
{
    public const string Package = "PACKAGE";
    public const string Plan = "PLAN";
}
