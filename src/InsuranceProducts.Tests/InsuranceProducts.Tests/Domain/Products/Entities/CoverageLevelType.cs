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
        public const string CoverageAmount = "COVERAGE_AMOUNT";
        public const string CoverageRange = "COVERAGE_RANGE";
        public const string Deductibility = "DEDUCTIBILITY";
        public const string Copay = "COPAY";
        public const string Coinsurance = "COINSURANCE";
        public const string CoverageLimit = "COVERAGE_LIMIT";

        public Code Code { get; protected set; }
        public Description Description { get; protected set; }

        protected CoverageLevelType(Builder builder) : base(builder.Id)
        {
            Id = builder.Id;
            Code = builder.Code;
        }

        protected CoverageLevelType() : base() => Code = default!;

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
                var newCoverageType = new CoverageLevelType(this);
                if (Description != null && !Description.IsEmpty)
                {
                    newCoverageType.UpdateDescription(Description);
                }
                return newCoverageType;
            }
        }
    }
}
