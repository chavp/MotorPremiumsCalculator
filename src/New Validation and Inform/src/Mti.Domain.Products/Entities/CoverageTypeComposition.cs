using EnsureThat;
using Mti.Domain.SharedKernel.Primatives;

namespace Mti.Domain.Products.Entities;

public sealed class CoverageTypeComposition : Entity<Guid>
{
    public Guid? FromCoverageTypeId { get; private set; }
    public CoverageType? FromCoverageType { get; private set; }

    public Guid? ToCoverageTypeId { get; private set; }
    public CoverageType? ToCoverageType { get; private set; }

    protected CoverageTypeComposition() { }

    protected CoverageTypeComposition(Builder builder) : base(builder.Id)
    {
        FromCoverageType = builder.FromCoverageType;
        ToCoverageType = builder.ToCoverageType;
    }

    public static Builder CreateBuilder(CoverageType? fromCoverageType, CoverageType? toCoverageType) 
        => new Builder(fromCoverageType, toCoverageType);

    public sealed class Builder
    {
        internal Guid Id { get; private set; }
        internal CoverageType FromCoverageType { get; private set; }
        internal CoverageType ToCoverageType { get; private set; }

        internal Builder(CoverageType? fromCoverageType, CoverageType? toCoverageType)
        {
            Ensure.That(fromCoverageType).IsNotNull();
            Ensure.That(toCoverageType).IsNotNull();

            Id = Guid.NewGuid();
            FromCoverageType = fromCoverageType;
            ToCoverageType = toCoverageType;
        }

        public CoverageTypeComposition Build()
        {
            var newEntity = new CoverageTypeComposition(this);
            return newEntity;
        }
    }
}
