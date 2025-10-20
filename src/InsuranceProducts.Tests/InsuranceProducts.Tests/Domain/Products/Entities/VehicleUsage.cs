using EnsureThat;
using InsuranceProducts.Tests.Domain.Products.ValueObjects;
using InsuranceProducts.Tests.Domain.SharedKernel;
using InsuranceProducts.Tests.Domain.SharedKernel.Primatives;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceProducts.Tests.Domain.Products.Entities
{
    public sealed class VehicleUsage : Entity<Guid>
    {
        public Code Code { get; protected set; }
        public Description Description { get; protected set; }

        protected VehicleUsage(
           Guid id,
           Code code) : base(id)
        {
            Id = Ensure.That(id).NotEmpty("Id is not empty", nameof(id));
            Code = Ensure.That(code).NotEmpty("Code is not empty", nameof(code));
        }

        protected VehicleUsage() : base() => Code = default!;

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

            public VehicleUsage Build()
            {
                var newEntity = new VehicleUsage(Id, Code);
                if (Description != null && !Description.IsEmpty)
                {
                    newEntity.UpdateDescription(Description);
                }
                return newEntity;
            }
        }
    }
}
