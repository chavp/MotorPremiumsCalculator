using Mti.Domain.Products.ValueObjects;
using Mti.Domain.SharedKernel.Primatives;

namespace Mti.Domain.Products.Entities;

public sealed class CoverageLimit : CoverageLevel
{
    public decimal Amount { get; private set; }

    public CoverageLimit SetAmount(decimal amount)
    {
        Amount = amount;
        return this;
    }

    protected CoverageLimit() { }

    protected internal CoverageLimit(Guid id,
        CoverageLevelType type,
        CoverageBasis basis,
        Unit unit) : base(id, type, basis, unit)
    {
    }
}
