using EnsureThat;
using Mti.Domain.Products.Extensions;
using Mti.Domain.Products.ValueObjects;
using Mti.Domain.SharedKernel.Primatives;

namespace Mti.Domain.Products.Entities;

public sealed class CoverageAvailabilityType : Entity<Guid>
{
    public const string Required = "REQUIRED";
    public const string Standard = "STANDARD";
    public const string Selectable = "SELECTABLE";
    public const string Optional = "OPTIONAL";

    public Code Code { get; private set; }
    public Description Description { get; private set; }

    protected CoverageAvailabilityType(Builder builder) : base(builder.Id)
    {
        Code = builder.Code;
        Description = builder.Description;
    }

    protected CoverageAvailabilityType() : base()
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
        internal Guid Id { get; private set; } = default!;
        internal Code Code { get; private set; } = default!;
        internal Description Description { get; private set; } = default!;

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

        public CoverageAvailabilityType Build()
        {
            var newEntity = new CoverageAvailabilityType(this);
            return newEntity;
        }
    }
}
