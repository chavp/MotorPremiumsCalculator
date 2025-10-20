using EnsureThat;
using Mti.Domain.Products.Extensions;
using Mti.Domain.Products.ValueObjects;
using Mti.Domain.SharedKernel.Primatives;

namespace Mti.Domain.Products.Entities
{
    public sealed class Campaign : Entity<Guid>
    {
        public Code Code { get; private set; }
        public Name Name { get; private set; }
        public Description Description { get; private set; }

        public Guid? PolicyTypeId { get; private set; }
        public PolicyType? PolicyType { get; private set; }
        public List<Product> Products { get; set; } = [];

        protected Campaign(Builder builder) : base(builder.Id)
        {
            PolicyType = builder.PolicyType;
            Code = builder.Code;
            Name = builder.Name;
            Description = builder.Description;
        }

        protected Campaign() : base()
        {
            Code = default!;
        }

        public Campaign UpdateName(string name)
        {
            Name = Name.Create(name);
            return this;
        }

        public Campaign UpdateDescription(string description)
        {
            Description = Description.Create(description);
            return this;
        }

        public static Builder CreateBuilder(PolicyType policyType, string code, string name) 
            => new Builder(Guid.NewGuid(), policyType, Code.Create(code), Name.Create(name));

        public sealed class Builder
        {
            internal Guid Id { get; private set; } = default!;
            internal Code Code { get; private set; } = default!;
            internal Name Name { get; private set; } = default!;
            internal Description Description { get; private set; } = default!;
            internal PolicyType? PolicyType { get; private set; }

            public Builder WithDescription(string description)
            {
                Description = Description.Create(description);
                return this;
            }

            internal Builder(Guid id, PolicyType policyType, Code code, Name name)
            {
                Id = Ensure.That(id).NotEmpty("Id is not empty", nameof(id));
                Code = Ensure.That(code).NotEmpty("Code is not empty", nameof(code));
                Name = Ensure.That(name).NotEmpty("Name is not empty", nameof(name));

                Ensure.That(policyType).IsNotNull();
                PolicyType = policyType;
            }

            public Campaign Build()
            {
                var newEntity = new Campaign(this);
                return newEntity;
            }
        }
    }
}
