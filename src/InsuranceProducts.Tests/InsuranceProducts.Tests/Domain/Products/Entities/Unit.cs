using EnsureThat;
using InsuranceProducts.Tests.Domain.Products.ValueObjects;
using InsuranceProducts.Tests.Domain.SharedKernel;
using InsuranceProducts.Tests.Domain.SharedKernel.Primatives;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceProducts.Tests.Domain.Products.Entities;

public sealed class Unit : Entity<Guid>
{
    public const string Baht = "BAHT";
    public const string Usd = "USD";

    public const string Day = "DAY";

    public Code Code { get; private set; }
    public Description Description { get; private set; }
    public Symbol? Symbol { get; private set; }

    public Guid? UnitCategoryId { get; private set; }
    public UnitCategory? UnitCategory { get; private set; }

    protected Unit(Builder builder) : base(builder.Id)
    {
        Id = builder.Id;
        Code = builder.Code;
        UnitCategory = builder.UnitCategory;
    }

    protected Unit() : base()
    {
        Code = default!;
    }

    public void UpdateDescription(string description)
    {
        Description = Description.Create(description);
    }
    public void UpdateSymbol(string symbol)
    {
        Symbol = Symbol.Create(symbol);
    }

    public static Builder CreateBuilder(Guid id, Code code, UnitCategory? unitCategory) => new Builder(id, code, unitCategory);

    public sealed class Builder
    {
        internal Guid Id { get; private set; } = default!;
        internal Code Code { get; private set; } = default!;
        internal Description Description { get; private set; } = default!;
        internal Symbol Symbol { get; private set; } = default!;
        internal UnitCategory? UnitCategory { get; private set; }

        public Builder WithDescription(string description)
        {
            Description = Description.Create(description);
            return this;
        }
        public Builder WithSymbol(string symbol)
        {
            Symbol = Symbol.Create(symbol);
            return this;
        }

        internal Builder(Guid id, Code code, UnitCategory? unitCategory)
        {
            Id = Ensure.That(id).NotEmpty("Id is not empty", nameof(id));
            Code = Ensure.That(code).NotEmpty("Code is not empty", nameof(code));
            Ensure.That(unitCategory).IsNotNull();
            UnitCategory = unitCategory;
        }

        public Unit Build()
        {
            var newEntity = new Unit(this);
            if (Description != null && !Description.IsEmpty)
            {
                newEntity.UpdateDescription(Description);
            }
            if (Symbol != null && !Symbol.IsEmpty)
            {
                newEntity.UpdateSymbol(Symbol);
            }
            return newEntity;
        }
    }
}
