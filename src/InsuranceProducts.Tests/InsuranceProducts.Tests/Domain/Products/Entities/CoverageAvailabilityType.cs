using EnsureThat;
using InsuranceProducts.Tests.Domain.Products.ValueObjects;
using InsuranceProducts.Tests.Domain.SharedKernel;
using InsuranceProducts.Tests.Domain.SharedKernel.Primatives;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceProducts.Tests.Domain.Products.Entities
{
    public sealed class CoverageAvailabilityType : Entity<Guid>
    {
        public const string Required = "REQUIRED";
        public const string Standard = "STANDARD";
        public const string Selectable = "SELECTABLE";
        public const string Optional = "OPTIONAL";

        public Code Code { get; protected set; }
        public Description Description { get; protected set; }

        protected CoverageAvailabilityType(Builder builder) : base(builder.Id)
        {
            Id = builder.Id;
            Code = builder.Code;
        }

        protected CoverageAvailabilityType() : base()
        {
            Code = default!;
        }

        public void UpdateDescription(string description)
        {
            Description = Description.Create(description);
        }

        public static Builder CreateBuilder(Guid id, Code code) => new Builder(id, code);

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

            public CoverageAvailabilityType Build()
            {
                var newEntity = new CoverageAvailabilityType(this);
                if (Description != null && !Description.IsEmpty)
                {
                    newEntity.UpdateDescription(Description);
                }
                return newEntity;
            }
        }
    }
}
