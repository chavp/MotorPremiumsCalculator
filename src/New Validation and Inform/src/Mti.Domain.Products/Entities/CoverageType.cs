using EnsureThat;
using Mti.Domain.Products.Extensions;
using Mti.Domain.Products.ValueObjects;
using Mti.Domain.SharedKernel.Primatives;

namespace Mti.Domain.Products.Entities;

public sealed class CoverageType : Entity<Guid>
{
    public Code Code { get; private set; }
    public Name Name { get; private set; }
    public Description Description { get; private set; }

    public IReadOnlyList<CoverageTypeComposition> FromCompositions { get; }
    public IReadOnlyList<CoverageTypeComposition> ToCompositions { get; }

    protected CoverageType(Builder builder) : base(builder.Id)
    {
        Id = builder.Id;
        Code = builder.Code;
        Name = builder.Name;
        Description = builder.Description;
    }

    public CoverageType() : base()
    {
        Code = default!;
    }

    public CoverageType UpdateName(string name)
    {
        Name = Name.Create(name);
        return this;
    }

    public CoverageType UpdateDescription(string description)
    {
        Description = Description.Create(description);
        return this;
    }

    public static Builder CreateBuilder(Code code) => new Builder(Guid.NewGuid(), code);

    public sealed class Builder
    {
        internal Guid Id { get; private set; } = default!;
        internal Code Code { get; private set; } = default!;
        internal Name Name { get; private set; } = default!;
        internal Description Description { get; private set; } = default!;

        public Builder WithName(string name)
        {
            Name = Name.Create(name);
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

        public CoverageType Build()
        {
            var newEntity = new CoverageType(this);
            return newEntity;
        }
    }
}
