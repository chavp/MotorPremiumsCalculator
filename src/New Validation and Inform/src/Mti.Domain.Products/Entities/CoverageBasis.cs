using EnsureThat;
using Mti.Domain.Products.Extensions;
using Mti.Domain.Products.ValueObjects;
using Mti.Domain.SharedKernel.Primatives;

namespace Mti.Domain.Products.Entities;

public sealed class CoverageBasis : Entity<Guid>
{
    public Code Code { get; private set; }
    public Description Description { get; private set; }

    protected CoverageBasis(Builder builder) : base(builder.Id)
    {
        Code = builder.Code;
        Description = builder.Description;
    }

    protected CoverageBasis() : base()
    {
        Code = default!;
    }

    public CoverageBasis UpdateDescription(string description)
    {
        Description = Description.Create(description);
        return this;
    }

    public static Builder CreateBuilder(Guid id,
        Code code) => new Builder(id, code);

    public sealed class Builder
    {
        internal Guid Id { get; private set; } = default!;
        internal Code Code { get; private set; } = default!;
        internal Description Description { get; private set; } = default!;

        public Builder WithCode(Code code)
        {
            Code = code;
            return this;
        }

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

        public CoverageBasis Build()
        {
            var newCoverageType = new CoverageBasis(this);
            return newCoverageType;
        }
    }
}
