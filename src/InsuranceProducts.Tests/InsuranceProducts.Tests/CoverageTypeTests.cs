using InsuranceProducts.Tests.Domain.Products.Entities;
using InsuranceProducts.Tests.Domain.Products.ValueObjects;
using InsuranceProducts.Tests.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InsuranceProducts.Tests
{
    public class CoverageTypeTests
    {
        string _con = "Host=localhost;Port=5432;Database=insurance-db;Username=admin;Password=admin123";

        ProductsDbContextFactory _factory;
        public CoverageTypeTests()
        {
            _factory = new ProductsDbContextFactory(_con);
        }

        Code Compulsory = Code.Create("COMPULSORY");
        Code Cov001 = Code.Create("COV001");
        Code PerPerson = Code.Create("PER_PERSON");
        Code CovAmountType = Code.Create("COVERAGE_AMOUNT");

        private void seedSimpleTest()
        {
            using var context = _factory.CreateDbContext();

            // Products
            var compulsory = Product.CreateBuilder(Guid.NewGuid(),
                Compulsory)
                .WithDescription("ประกันภัยคุ้มครองผู้ประสบภัยจำกรถ")
                .Build();

            context.Add(compulsory);

            // Coverages
            // BODILY_INJURY_OR_INJURY_TO_HEALTH
            var covType1 = CoverageType.CreateBuilder(Guid.NewGuid(), Cov001)
                .WithDescription("ความเสียหายต่อร่างกายหรืออนามัย")
                .Build();
            context.Add(covType1);

            var covBasis1 = new CoverageBasis(Guid.NewGuid(), PerPerson);
            covBasis1.UpdateDescription("ต่อหนึ่งคน");
            context.Add(covBasis1);

            var covLevel1 = new CoverageLevelType(Guid.NewGuid(), CovAmountType);
            context.Add(covLevel1);

            var covAmt = new CoverageAmount(Guid.NewGuid(), covLevel1, covBasis1, 80000);
            context.Add(covAmt);

            // CoverageAvailability
            var requiredAvailability = CoverageAvailabilityType.CreateBuilder(Guid.NewGuid(),
                Code.Create(CoverageAvailabilityType.Required))
                .WithDescription("กำหนดความคุ้มครองตามกฏกมายกำหนด")
                .Build();

            context.Add(requiredAvailability);

            context.SaveChanges();
        }


        [Fact]
        public void SeedSimpleTest()
        {
            seedSimpleTest();
        }

        [Fact]
        public void AddCovAvailability()
        {
            using var context = _factory.CreateDbContext();

            var comp = context.Products.Single(x => x.Code.Value == Compulsory.Value);
            var availRequiredType = context.CoverageAvailabilityTypes.Single(x => x.Code.Value == CoverageAvailabilityType.Required);
            var covType = context.CoverageTypes.Single(x => x.Code.Value == Cov001.Value);
            var covLevel = context.CoverageLevels
                .OfType<CoverageAmount>()
                .Single(x => x.CoverageLevelType.Code.Value == CovAmountType.Value
                && x.Amount == 80000);

            var requiredAvail = new CoverageAvailability(Guid.NewGuid(),
                comp, availRequiredType, covType, covLevel);
            context.Add(requiredAvail);
            context.SaveChanges();
        }

        [Fact]
        public void Display()
        {
            using var context = _factory.CreateDbContext();

            var comp = context
                .Products
                .Include(x => x.CoverageAvailabilities)
                    .ThenInclude(y => y.CoverageAvailabilityType)
                .Include(x => x.CoverageAvailabilities)
                    .ThenInclude(y => y.CoverageType)
                .Include(x => x.CoverageAvailabilities)
                    .ThenInclude(y => y.CoverageLevel)
                        .ThenInclude(z => z.CoverageLevelType)
                .Include(x => x.CoverageAvailabilities)
                    .ThenInclude(y => y.CoverageLevel)
                        .ThenInclude(z => z.CoverageBasis)
                .AsSplitQuery()
                .Single(x => x.Code.Value == Compulsory.Value);


            var report = "";
            var requiredCoves = comp.CoverageAvailabilities
                .Where(x => x.CoverageAvailabilityType.Code == CoverageAvailabilityType.Required);
            foreach (var covAvai in requiredCoves)
            {
                var coverType = covAvai.CoverageType;
                if (covAvai.CoverageLevel is CoverageAmount cavLevel)
                {
                    report += $"- {cavLevel.Amount} บาท {cavLevel.CoverageBasis.Description} สำหรับ{coverType.Description}\n";
                }
            }

            var sum = report;
        }
    }
}
