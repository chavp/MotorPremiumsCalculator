using EnsureThat;
using InsuranceProducts.Tests.Domain.Products.ValueObjects;
using InsuranceProducts.Tests.Domain.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceProducts.Tests.Domain.Products.Entities;

public class Product : Entity<Guid>
{
    public Code Code { get; protected set; }
    public Description Description { get; protected set; }

    public List<CoverageAvailability> CoverageAvailabilities { get; set; } = [];

    protected Product(Builder builder) : base(builder.Id)
    {
        Id = builder.Id;
        Code = builder.Code;
    }

    protected Product() : base()
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

        public Product Build()
        {
            var newEntity = new Product(this);
            if (Description != null && !Description.IsEmpty)
            {
                newEntity.UpdateDescription(Description);
            }
            return newEntity;
        }
    }
}
