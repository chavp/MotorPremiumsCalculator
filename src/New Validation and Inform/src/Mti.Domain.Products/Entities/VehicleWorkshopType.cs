using EnsureThat;
using Mti.Domain.Products.Extensions;
using Mti.Domain.Products.ValueObjects;
using Mti.Domain.SharedKernel.Primatives;

namespace Mti.Domain.Products.Entities;

public sealed class VehicleWorkshopType : Entity<Guid>
{
    public const string Garage = "GARAGE";
    public const string Dealer = "DEALER";

    public Code Code { get; private set; }
    public Name Name { get; private set; }
    public string LookupNames { get; private set; }

    protected VehicleWorkshopType(Builder builder) : base(builder.Id)
    {
        Code = builder.Code;
        Name = builder.Name;
        LookupNames = builder.LookupNames;
    }

    protected VehicleWorkshopType() : base()
    {
        Code = default!;
    }

    public VehicleWorkshopType UpdateLookupNames(params string[] lookupNames)
    {
        LookupNames = string.Join("|", lookupNames);
        return this;
    }

    public VehicleWorkshopType UpdateName(string name)
    {
        Name = Name.Create(name);
        return this;
    }

    public static Builder CreateBuilder(Code code, Name name) => new Builder(Guid.NewGuid(), code, name);

    public sealed class Builder
    {
        internal Guid Id { get; set; } = default!;
        internal Code Code { get; set; } = default!;
        internal Name Name { get; set; } = default!;
        internal string LookupNames { get; set; }

        public Builder WithName(string name)
        {
            Name = Name.Create(name);
            return this;
        }

        public Builder WithLookupNames(params string[] lookupNames)
        {
            LookupNames = string.Join("|", lookupNames);
            return this;
        }

        internal Builder(Guid id, Code code, Name name)
        {
            Id = Ensure.That(id).NotEmpty("Id is not empty", nameof(id));
            Code = Ensure.That(code).NotEmpty("Code is not empty", nameof(code));
            Name = Ensure.That(name).NotEmpty("Name is not empty", nameof(name));
        }

        public VehicleWorkshopType Build() => new VehicleWorkshopType(this);
    }
}
