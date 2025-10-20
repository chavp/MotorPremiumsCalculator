using EnsureThat;
using Mti.Domain.Products.Extensions;
using Mti.Domain.Products.ValueObjects;
using Mti.Domain.SharedKernel.Primatives;

namespace Mti.Domain.Products.Entities;

public sealed class VehicleFuelType : Entity<Guid>
{
    public const string Combustion = "COMBUSTION";
    public const string Electric = "ELECTRIC";

    public Code Code { get; private set; }
    public Name Name { get; private set; }
    public Prefix Prefix { get; private set; }

    protected VehicleFuelType(Builder builder) : base(builder.Id)
    {
        Code = builder.Code;
        Name = builder.Name;
        Prefix = builder.Prefix;
    }

    protected VehicleFuelType() : base()
    {
        Code = default!;
    }

    public VehicleFuelType UpdatePrefix(string prefix)
    {
        Prefix = Prefix.Create(prefix);
        return this;
    }

    public VehicleFuelType UpdateName(string name)
    {
        Name = Name.Create(name);
        return this;
    }

    public static Builder CreateBuilder(Code code) => new Builder(Guid.NewGuid(), code);

    public sealed class Builder
    {
        internal Guid Id { get; private set; } = default!;
        internal Code Code { get; private set; } = default!;
        internal Name Name { get; private set; } = default!;
        internal Prefix Prefix { get; private set; } = default!;

        public Builder WithName(string name)
        {
            Name = Name.Create(name);
            return this;
        }

        public Builder WithPrefix(string prefix)
        {
            Prefix = Prefix.Create(prefix);
            return this;
        }

        internal Builder(Guid id, Code code)
        {
            Id = Ensure.That(id).NotEmpty("Id is not empty", nameof(id));
            Code = Ensure.That(code).NotEmpty("Code is not empty", nameof(code));
        }

        public VehicleFuelType Build() => new VehicleFuelType(this);
    }
}
