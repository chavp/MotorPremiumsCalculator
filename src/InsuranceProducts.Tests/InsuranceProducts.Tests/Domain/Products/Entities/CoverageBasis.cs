using EnsureThat;
using InsuranceProducts.Tests.Domain.Products.ValueObjects;
using InsuranceProducts.Tests.Domain.SharedKernel;
using InsuranceProducts.Tests.Domain.SharedKernel.Primatives;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceProducts.Tests.Domain.Products.Entities
{
    public sealed class CoverageBasis : Entity<Guid>
    {
        public Code Code { get; protected set; }
        public Description Description { get; protected set; }

        public CoverageBasis(
        Guid id,
        Code code) : base(id)
        {
            Id = Ensure.That(id).NotEmpty("Id is not empty", nameof(id));
            Code = Ensure.That(code).NotEmpty("Code is not empty", nameof(code));
        }

        protected CoverageBasis() : base()
        {
            Code = default!;
        }

        public void UpdateDescription(string description)
        {
            Description = Description.Create(description);
        }

        public static CoverageBasis Create(
            Guid id,
            Code code,
            string description)
        {
            var coverageType = new CoverageBasis(id, code);
            if (!string.IsNullOrWhiteSpace(description))
            {
                coverageType.UpdateDescription(description);
            }
            return coverageType;
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

            public CoverageBasis Build()
            {
                var newCoverageType = new CoverageBasis(Id, Code);
                if (Description != null && !Description.IsEmpty)
                {
                    newCoverageType.UpdateDescription(Description);
                }
                return newCoverageType;
            }
        }
    }
}
