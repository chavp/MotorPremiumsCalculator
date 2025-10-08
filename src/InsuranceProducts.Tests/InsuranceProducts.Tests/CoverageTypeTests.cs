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
        Code Cov002 = Code.Create("COV002");
        Code PerPerson = Code.Create("PER_PERSON");
        Code CovAmountType = Code.Create("COVERAGE_AMOUNT");

        [Fact]
        public void SeedSimpleTest()
        {
            using var context = _factory.CreateDbContext();

            // Units
            var timeCat = UnitCategory
                .CreateBuilder(Guid.NewGuid(), Code.Create(UnitCategory.Time))
                .Build();
            context.Add(timeCat);
            var currencyCat = UnitCategory
                .CreateBuilder(Guid.NewGuid(), Code.Create(UnitCategory.Currency))
                .Build();
            context.Add(currencyCat);

            var bth = Unit.CreateBuilder(Guid.NewGuid(), Code.Create(Unit.Baht), currencyCat)
                .WithSymbol("฿")
                .Build();
            context.Add(bth);
            var usd = Unit.CreateBuilder(Guid.NewGuid(), Code.Create(Unit.Usd), currencyCat)
                .WithSymbol("$")
                .Build();
            context.Add(usd);
            var day = Unit.CreateBuilder(Guid.NewGuid(), Code.Create(Unit.Day), timeCat)
                .Build();
            context.Add(day);

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
            var covType2 = CoverageType.CreateBuilder(Guid.NewGuid(), Cov002)
                .WithDescription("สำหรับการเสียชีวิต หรือทุพพลภำพถำวรสิ้นเชิง")
                .Build();
            context.Add(covType2);

            var covBasis1 = new CoverageBasis(Guid.NewGuid(), PerPerson);
            covBasis1.UpdateDescription("ต่อหนึ่งคน");
            context.Add(covBasis1);

            var covLevel1 = new CoverageLevelType(Guid.NewGuid(), CovAmountType);
            context.Add(covLevel1);

            var covAmt1 = new CoverageAmount(Guid.NewGuid(), covLevel1, covBasis1, bth, 80000);
            context.Add(covAmt1);
            var covAmt2 = new CoverageAmount(Guid.NewGuid(), covLevel1, covBasis1, bth, 500000);
            context.Add(covAmt2);

            // CoverageAvailability
            var requiredAvailabilityType = CoverageAvailabilityType.CreateBuilder(Guid.NewGuid(),
                Code.Create(CoverageAvailabilityType.Required))
                .WithDescription("กำหนดความคุ้มครองตามกฏกมายกำหนด")
                .Build();
            context.Add(requiredAvailabilityType);

            context.SaveChanges();
        }

        [Fact]
        public void AddCovAvailability()
        {
            using var context = _factory.CreateDbContext();

            var comp = context.Products.Single(x => x.Code.Value == Compulsory.Value);
            var availRequiredType = context.CoverageAvailabilityTypes.Single(x => x.Code.Value == CoverageAvailabilityType.Required);
            
            addCovAvail(context, comp, availRequiredType, Cov001.Value, 80000);
            addCovAvail(context, comp, availRequiredType, Cov002.Value, 500000);

            context.SaveChanges();
        }

        private void addCovAvail(ProductsDbContext context, 
            Product product, 
            CoverageAvailabilityType covAvaiType,
            string covTypeCode,
            decimal amount)
        {
            var cov1Type = context.CoverageTypes.Single(x => x.Code.Value == covTypeCode);
            var covLevel1 = context.CoverageLevels
                .OfType<CoverageAmount>()
                .Single(x => x.CoverageLevelType.Code.Value == CovAmountType.Value
                && x.Amount == amount);
            var requiredAvail = new CoverageAvailability(Guid.NewGuid(),
                product, covAvaiType, cov1Type, covLevel1);

            context.Add(requiredAvail);
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
                .Include(x => x.CoverageAvailabilities)
                    .ThenInclude(y => y.CoverageLevel)
                        .ThenInclude(z => z.Unit)
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
                    report += $"- {cavLevel.Unit.Symbol}{cavLevel.Amount} {cavLevel.CoverageBasis.Description} สำหรับ{coverType.Description}\n";
                }
            }

            var sum = report;
        }
    }
}
