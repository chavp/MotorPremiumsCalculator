using EnsureThat;
using Mti.Domain.Products.Extensions;
using Mti.Domain.Products.ValueObjects;
using Mti.Domain.SharedKernel.Primatives;

namespace Mti.Domain.Products.Entities;

public sealed class CoverageLevelType : Entity<Guid>
{
    public const string CoverageAmount = "COVERAGE_AMOUNT";
    public const string CoverageRange = "COVERAGE_RANGE";
    public const string Deductibility = "DEDUCTIBILITY";
    public const string Copay = "COPAY";
    public const string Coinsurance = "COINSURANCE";
    public const string CoverageLimit = "COVERAGE_LIMIT";

    public Code Code { get; private set; }
    public Description Description { get; private set; }

    protected CoverageLevelType(Builder builder) : base(builder.Id)
    {
        Id = builder.Id;
        Code = builder.Code;
        Description = builder.Description;
    }

    protected CoverageLevelType() : base() => Code = default!;

    public CoverageLevelType UpdateDescription(string description)
    {
        Description = Description.Create(description);
        return this;
    }

    public static Builder CreateBuilder(Code code) => new Builder(Guid.NewGuid(), code);

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

        public CoverageLevelType Build()
        {
            var newCoverageType = new CoverageLevelType(this);
            return newCoverageType;
        }
    }
}
