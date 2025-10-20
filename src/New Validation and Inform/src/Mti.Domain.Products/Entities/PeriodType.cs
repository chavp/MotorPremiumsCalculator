using EnsureThat;
using Mti.Domain.Products.Extensions;
using Mti.Domain.Products.ValueObjects;
using Mti.Domain.SharedKernel.Primatives;

namespace Mti.Domain.Products.Entities;

public sealed class PeriodType : Entity<Guid>
{
    public const string Yearly = "YEARLY";
    public const string Monthly = "MONTHLY";
    public const string Daily = "DAILY";

    public Code Code { get; private set; }
    public Name Name { get; private set; }

    protected PeriodType(Builder builder) : base(builder.Id)
    {
        Code = builder.Code;
        Name = builder.Name;
    }

    protected PeriodType() : base()
    {
        Code = default!;
    }

    public void UpdateName(string name)
    {
        Name = Name.Create(name);
    }

    public static Builder CreateBuilder(string code) => new Builder(Guid.NewGuid(), Code.Create(code));

    public sealed class Builder
    {
        internal Guid Id { get; private set; } = default!;
        internal Code Code { get; private set; } = default!;
        internal Name Name { get; private set; } = default!;

        public Builder WithName(string name)
        {
            Name = Name.Create(name);
            return this;
        }

        internal Builder(Guid id, Code code)
        {
            Id = Ensure.That(id).NotEmpty("Id is not empty", nameof(id));
            Code = Ensure.That(code).NotEmpty("Code is not empty", nameof(code));
        }

        public PeriodType Build()
        {
            var newEntity = new PeriodType(this);
            return newEntity;
        }
    }
}
