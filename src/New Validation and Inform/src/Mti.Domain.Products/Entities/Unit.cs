using EnsureThat;
using Mti.Domain.Products.Extensions;
using Mti.Domain.Products.ValueObjects;
using Mti.Domain.SharedKernel.Primatives;

namespace Mti.Domain.Products.Entities;

public sealed class Unit : Entity<Guid>
{
    public const string Baht = "BAHT";
    public const string Usd = "USD";

    public const string Day = "DAY";

    public const string Seat = "SEAT";
    public const string CubicCentimeter = "CC";
    public const string Kilogram = "KG";
    public const string MetricTon = "TON";
    public const string Horsepower = "HP";

    public Code Code { get; private set; }
    public Name Name { get; private set; }
    public Symbol? Symbol { get; private set; }

    public Guid? UnitCategoryId { get; private set; }
    public UnitCategory? UnitCategory { get; private set; }

    protected Unit(Builder builder) : base(builder.Id)
    {
        Id = builder.Id;
        Code = builder.Code;
        Name = builder.Name;
        Symbol = builder.Symbol;
        UnitCategory = builder.UnitCategory;
    }

    protected Unit() : base()
    {
        Code = default!;
    }

    public void UpdateName(string name)
    {
        Name = Name.Create(name);
    }
    public void UpdateSymbol(string symbol)
    {
        Symbol = Symbol.Create(symbol);
    }

    public static Builder CreateBuilder(Code code, UnitCategory? unitCategory)
        => new Builder(Guid.NewGuid(), code, unitCategory);

    public sealed class Builder
    {
        internal Guid Id { get; private set; } = default!;
        internal Code Code { get; private set; } = default!;
        internal Name Name { get; private set; } = default!;
        internal Symbol Symbol { get; private set; } = default!;
        internal UnitCategory? UnitCategory { get; private set; }

        public Builder WithName(string name)
        {
            Name = Name.Create(name);
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
            return newEntity;
        }
    }
}
