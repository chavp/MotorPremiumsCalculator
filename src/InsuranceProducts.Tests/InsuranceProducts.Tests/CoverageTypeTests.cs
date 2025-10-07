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

        [Fact]
        public void AddCoverage()
        {
            using var context = _factory.CreateDbContext();

            // BODILY_INJURY_OR_INJURY_TO_HEALTH
            var code = Code.Create("COV001");
            var covAmy = context.CoverageLevels
                .OfType<CoverageAmount>()
                .Include(x => x.CoverageBasis)
                .Include(x => x.CoverageLevelType)
                .SingleOrDefault(x => x.Amount == 80000 && x.CoverageBasis.Code.Value == "PER_PERSON");

            var covType1 = CoverageType.CreateBuilder(Guid.NewGuid(), code, covAmy)
                .WithDescription("ความเสียหายต่อร่างกายหรืออนามัย")
                .Build();
            context.Add(covType1);

            context.SaveChanges();
        }

        [Fact]
        public void AddLevelAndBasis()
        {
            using var context = _factory.CreateDbContext();

            //var cov = context.CoverageTypes.SingleOrDefault(x => x.Code.Value == "cov001");

            var perPerson = Code.Create("PER_PERSON");
            var covBasis1 = new CoverageBasis(Guid.NewGuid(), Code.Create("PER_PERSON"));
            covBasis1.UpdateDescription("ต่อหนึ่งคน");
            context.Add(covBasis1);

            var covAmountType = Code.Create("COVERAGE_AMOUNT");
            var covLevel1 = new CoverageLevelType(Guid.NewGuid(), Code.Create("COVERAGE_AMOUNT"));
            context.Add(covLevel1);

            context.SaveChanges();
        }

        [Fact]
        public void AddLevel()
        {
            using var context = _factory.CreateDbContext();

            //var cov = context.CoverageTypes.SingleOrDefault(x => x.Code.Value == "cov001");

            var perPerson = Code.Create("PER_PERSON");
            var covBasis1 = context.CoverageBasises.Single(x => x.Code.Value == perPerson);

            //var covBasis1 = new CoverageBasis(Guid.NewGuid(), Code.Create("PER_PERSON"));
            //covBasis1.UpdateDescription("ต่อหนึ่งคน");
            //context.Add(covBasis1);

            var covAmountType = Code.Create("COVERAGE_AMOUNT");
            var covLevel1 = context.CoverageLevelTypes.Single(x => x.Code.Value == covAmountType);
            //var covLevel1 = new CoverageLevelType(Guid.NewGuid(), Code.Create("COVERAGE_AMOUNT"));
            //context.Add(covLevel1);

            var covAmt = new CoverageAmount(Guid.NewGuid(), covLevel1, covBasis1, 80000);
            context.Add(covAmt);

            context.SaveChanges();
        }

        [Fact]
        public void Display()
        {
            using var context = _factory.CreateDbContext();

            var coverTypes = context.CoverageTypes
                .ToList();

            var report = "";
            foreach (var coverType in coverTypes)
            {
                var cavLevel = context.CoverageLevels
                    .OfType<CoverageAmount>()
                    .Include(x => x.CoverageBasis)
                    .Include(x => x.CoverageLevelType)
                    .Where(x => x.Id == Guid.Parse("e9d93d09-ca04-4828-8fa3-5011fb6a346f"))
                    .Single();

                report += $"- {cavLevel.Amount} บาท {cavLevel.CoverageBasis.Description} สำหรับ{coverType.Description}\n";
            }

            var sum = report;
        }
    }
}
