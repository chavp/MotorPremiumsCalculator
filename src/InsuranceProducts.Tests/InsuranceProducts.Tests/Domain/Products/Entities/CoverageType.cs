using EnsureThat;
using InsuranceProducts.Tests.Domain.Products.ValueObjects;
using InsuranceProducts.Tests.Domain.SharedKernel;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace InsuranceProducts.Tests.Domain.Products.Entities;

public class CoverageType : Entity<Guid>
{
    public Code Code { get; protected set; }
    public Description Description { get; protected set; }

    protected CoverageType(
        Guid id,
        Code code) : base(id)
    {
        Id = Ensure.That(id).NotEmpty("Id is not empty", nameof(id));
        Code = Ensure.That(code).NotEmpty("Code is not empty", nameof(code));
    }

    protected CoverageType() : base()
    {
        Code = default!;
    }

    public void UpdateDescription(string description)
    {
        Description = Description.Create(description);
    }

    public static CoverageType Create(
        Guid id,
        Code code,
        CoverageLevel level,
        string description)
    {
        var coverageType = new CoverageType(id, code);
        if (!string.IsNullOrWhiteSpace(description))
        {
            coverageType.UpdateDescription(description);
        }
        return coverageType;
    }

    public static Builder CreateBuilder(Guid id,
        Code code, CoverageLevel level) => new Builder(id, code, level);

    public sealed class Builder
    {
        internal Guid Id { get; set; } = default!;
        internal Code Code { get; set; } = default!;
        internal CoverageLevel CoverageLevel { get; set; } = default!;
        internal Description Description { get; set; } = default!;

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

        public Builder(Guid id, Code code, CoverageLevel level)
        {
            Id = Ensure.That(id).NotEmpty("Id is not empty", nameof(id));
            Code = Ensure.That(code).NotEmpty("Code is not empty", nameof(code));
            CoverageLevel = level;
        }

        public CoverageType Build()
        {
            var newCoverageType = new CoverageType(Id, Code);
            if (Description != null && !Description.IsEmpty)
            {
                newCoverageType.UpdateDescription(Description);
            }
            return newCoverageType;
        }
    }
}
