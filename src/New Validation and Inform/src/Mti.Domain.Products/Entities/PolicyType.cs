using EnsureThat;
using Mti.Domain.Products.Extensions;
using Mti.Domain.Products.ValueObjects;
using Mti.Domain.SharedKernel.Primatives;

namespace Mti.Domain.Products.Entities
{
    public sealed class PolicyType : Entity<Guid>
    {
        public Code Code { get; private set; }
        public Name Name { get; private set; }

        protected PolicyType(Builder builder) : base(builder.Id)
        {
            Code = builder.Code;
            Name = builder.Name;
        }

        protected PolicyType() : base()
        {
            Code = default!;
        }

        public void UpdateName(string name)
        {
            Name = Name.Create(name);
        }

        public static Builder CreateBuilder(string code, string name)
            => new Builder(Guid.NewGuid(), Code.Create(code), Name.Create(name));

        public sealed class Builder
        {
            internal Guid Id { get; private set; } = default!;
            internal Code Code { get; private set; } = default!;
            internal Name Name { get; private set; } = default!;

            internal Builder(Guid id, Code code, Name name)
            {
                Id = Ensure.That(id).NotEmpty("Id is not empty", nameof(id));
                Code = Ensure.That(code).NotEmpty("Code is not empty", nameof(code));
                Name = Ensure.That(name).NotEmpty("Name is not empty", nameof(name));
            }

            public PolicyType Build()
            {
                var newEntity = new PolicyType(this);
                return newEntity;
            }
        }
    }
}
