using EnsureThat;
using InsuranceProducts.Tests.Domain.Products.ValueObjects;
using InsuranceProducts.Tests.Domain.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceProducts.Tests.Domain.Products.Entities
{
    public sealed class CoverageLevelType : Entity<Guid>
    {
        public Code Code { get; protected set; }
        public Description Description { get; protected set; }

        public CoverageLevelType(
           Guid id,
           Code code) : base(id)
        {
            Id = Ensure.That(id).NotEmpty("Id is not empty", nameof(id));
            Code = Ensure.That(code).NotEmpty("Code is not empty", nameof(code));
        }

        protected CoverageLevelType() : base() => Code = default!;

        public void UpdateDescription(string description)
        {
            Description = Description.Create(description);
        }

        public static Builder CreateBuilder(Guid id,
            Code code) => new Builder(id, code);

        public sealed class Builder
        {
            internal Guid Id { get; set; } = default!;
            internal Code Code { get; set; } = default!;
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

            public Builder(Guid id, Code code)
            {
                Id = Ensure.That(id).NotEmpty("Id is not empty", nameof(id));
                Code = Ensure.That(code).NotEmpty("Code is not empty", nameof(code));
            }

            public CoverageLevelType Build()
            {
                var newCoverageType = new CoverageLevelType(Id, Code);
                if (Description != null && !Description.IsEmpty)
                {
                    newCoverageType.UpdateDescription(Description);
                }
                return newCoverageType;
            }
        }
    }
}
