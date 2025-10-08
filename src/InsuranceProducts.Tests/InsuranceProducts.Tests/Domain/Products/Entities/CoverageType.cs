using EnsureThat;
using InsuranceProducts.Tests.Domain.Products.ValueObjects;
using InsuranceProducts.Tests.Domain.SharedKernel;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace InsuranceProducts.Tests.Domain.Products.Entities;

public sealed class CoverageType : Entity<Guid>
{
    public Code Code { get; protected set; }
    public Description Description { get; protected set; }

    protected CoverageType(Builder builder) : base(builder.Id)
    {
        Id = builder.Id;
        Code = builder.Code;
    }

    protected CoverageType() : base()
    {
        Code = default!;
    }

    public void UpdateDescription(string description)
    {
        Description = Description.Create(description);
    }

    public static Builder CreateBuilder(Guid id,Code code) => new Builder(id, code);

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
