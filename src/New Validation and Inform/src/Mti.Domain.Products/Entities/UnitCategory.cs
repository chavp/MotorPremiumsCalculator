using EnsureThat;
using Mti.Domain.Products.Extensions;
using Mti.Domain.Products.ValueObjects;
using Mti.Domain.SharedKernel.Primatives;

namespace Mti.Domain.Products.Entities;

public sealed class UnitCategory : Entity<Guid>
{
    public const string Time = "TIME";
    public const string Currency = "CURRENCY";
    public const string Quantity = "QUANTITY";
    public const string Weight = "WEIGHT";
    public const string Volume = "VOLUME";
    public const string Power = "POWER";

    public Code Code { get; protected set; }
    public Name Name { get; protected set; }

    public UnitCategory(
       Guid id,
       Code code) : base(id)
    {
        Id = Ensure.That(id).NotEmpty("Id is not empty", nameof(id));
        Code = Ensure.That(code).NotEmpty("Code is not empty", nameof(code));
    }

    protected UnitCategory() : base() => Code = default!;

    public void UpdateName(string name)
    {
        Name = Name.Create(name);
    }

    public static Builder CreateBuilder(Code code) => new Builder(Guid.NewGuid(), code);

    public sealed class Builder
    {
        internal Guid Id { get; set; } = default!;
        internal Code Code { get; set; } = default!;
        internal Name Name { get; set; } = default!;

        public Builder WithCode(Code code)
        {
            Code = code;
            return this;
        }

        public Builder WithName(string name)
        {
            Name = Name.Create(name);
            return this;
        }

        public Builder(Guid id, Code code)
        {
            Id = Ensure.That(id).NotEmpty("Id is not empty", nameof(id));
            Code = Ensure.That(code).NotEmpty("Code is not empty", nameof(code));
        }

        public UnitCategory Build()
        {
            var newEntity = new UnitCategory(Id, Code);
            if (Name != null && !Name.IsEmpty)
            {
                newEntity.UpdateName(Name);
            }
            return newEntity;
        }
    }
}
