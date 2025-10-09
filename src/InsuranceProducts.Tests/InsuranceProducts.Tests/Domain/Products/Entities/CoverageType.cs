using EnsureThat;
using InsuranceProducts.Tests.Domain.Products.ValueObjects;
using InsuranceProducts.Tests.Domain.SharedKernel;

namespace InsuranceProducts.Tests.Domain.Products.Entities;

public sealed class CoverageType : Entity<Guid>
{
    public Code Code { get; protected set; }
    public Description Description { get; protected set; }

    public IReadOnlyList<CoverageTypeComposition> FromCompositions { get; }
    public IReadOnlyList<CoverageTypeComposition> ToCompositions { get; }

    protected CoverageType(Builder builder) : base(builder.Id)
    {
        Id = builder.Id;
        Code = builder.Code;
    }

    public CoverageType() : base()
    {
        Code = default!;
    }

    public void UpdateDescription(string description)
    {
        Description = Description.Create(description);
    }

    public static Builder CreateBuilder(Code code) => new Builder(Guid.NewGuid(), code);

    public sealed class Builder
    {
        internal Guid Id { get; set; } = default!;
        internal Code Code { get; set; } = default!;
        internal Description Description { get; set; } = default!;

        public Builder WithDescription(string description)
        {
            Description = Description.Create(description);
            return this;
        }

        internal Builder(Guid id, Code code)
        {
            Id = Ensure.That(id).NotEmpty("Id is not empty", nameof(id));
            Code = Ensure.That(code).NotEmpty("Code is not empty", nameof(code));
        }

        public CoverageType Build()
        {
            var newEntity = new CoverageType(this);
            if (Description != null && !Description.IsEmpty)
            {
                newEntity.UpdateDescription(Description);
            }
            return newEntity;
        }
    }
}
