using EnsureThat;
using Mti.Domain.Products.Extensions;
using Mti.Domain.Products.ValueObjects;
using Mti.Domain.SharedKernel.Primatives;

namespace Mti.Domain.Products.Entities;

public class Product : Entity<Guid>
{
    public Code Code { get; private set; }
    public Name Name { get; private set; }
    public Description Description { get; private set; }

    public DateOnly SaleStartDate { get; private set; } = DateOnly.FromDateTime(DateTime.Now);
    public DateOnly SaleEndDate { get; private set; } = DateOnly.MaxValue;

    public List<CoverageAvailability> CoverageAvailabilities { get; private set; } = [];
    public List<ProductFeatureAvailability> ProductFeatureAvailabilities { get; private set; } = [];
    //public List<CampaignProduct> CampaignProducts { get; set; } = [];

    public List<Campaign> Campaigns { get; set; } = [];

    protected Product(Builder builder) : base(builder.Id)
    {
        Code = builder.Code;
        Name = builder.Name;
        Description = builder.Description;
        SaleStartDate = builder.SaleStartDate;
        SaleEndDate = builder.SaleEndDate;
    }

    protected Product() : base()
    {
        Code = default!;
        Name = default!;
    }

    public Product UpdateName(string name)
    {
        Name = Name.Create(name);
        return this;
    }

    public Product UpdateDescription(string description)
    {
        Description = Description.Create(description);
        return this;
    }

    public Product UpdateSaleStartDate(DateOnly saleStartDate)
    {
        SaleStartDate = saleStartDate;
        return this;
    }
    public Product UpdateSaleEndDate(DateOnly saleEndDate)
    {
        SaleEndDate = saleEndDate;
        return this;
    }

    public static Builder CreateBuilder(string code, string name) 
        => new Builder(Guid.NewGuid(), Code.Create(code), Name.Create(name));

    public sealed class Builder
    {
        internal Guid Id { get; private set; } = default!;
        internal Code Code { get; private set; } = default!;
        internal Name Name { get; private set; } = default!;
        internal Description Description { get; private set; } = default!;
        internal DateOnly SaleStartDate { get; private set; } = DateOnly.FromDateTime(DateTime.Now);
        internal DateOnly SaleEndDate { get; private set; } = DateOnly.MaxValue;

        public Builder WithDescription(string description)
        {
            Description = Description.Create(description);
            return this;
        }
        public Builder WithSaleStartDate(DateOnly saleStartDate)
        {
            SaleStartDate = saleStartDate;
            return this;
        }
        public Builder WithSaleEndDate(DateOnly saleEndDate)
        {
            SaleEndDate = saleEndDate;
            return this;
        }

        internal Builder(Guid id, Code code, Name name)
        {
            Id = Ensure.That(id).NotEmpty("Id is not empty", nameof(id));
            Code = Ensure.That(code).NotEmpty("Code is not empty", nameof(code));
            Name = Ensure.That(name).NotEmpty("Name is not empty", nameof(name));
        }

        public Product Build()
        {
            var newEntity = new Product(this);
            return newEntity;
        }
    }
}
