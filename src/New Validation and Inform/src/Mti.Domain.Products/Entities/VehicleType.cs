using EnsureThat;
using Mti.Domain.Products.Extensions;
using Mti.Domain.Products.ValueObjects;
using Mti.Domain.SharedKernel.Primatives;

namespace Mti.Domain.Products.Entities
{
    public sealed class VehicleType : Entity<Guid>
    {
        public Code Code { get; protected set; }
        public Name Name { get; protected set; }

        protected VehicleType(Builder builder) : base(builder.Id)
        {
            Id = builder.Id;
            Code = builder.Code;
        }

        protected VehicleType() : base() => Code = default!;

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

            public VehicleType Build()
            {
                var newEntity = new VehicleType(this);
                if (Name != null && !Name.IsEmpty)
                {
                    newEntity.UpdateName(Name);
                }
                return newEntity;
            }
        }
    }
}
