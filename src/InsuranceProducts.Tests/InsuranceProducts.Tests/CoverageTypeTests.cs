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
        Code Cov001 = Code.Create("COV001"); // ควำมเสียหำยต่อร่ำงกำยหรืออนำมัย
        Code Cov002 = Code.Create("COV002"); // กำรเสียชีวิต หรือทุพพลภำพถำวรสิ้นเชิง
        Code Cov003 = Code.Create("COV003"); // ทุพพลภำพอย่ำงถำวร หรือกำรสูญเสียอวัยวะ
        Code Cov004 = Code.Create("COV004"); // ชดเชยรำยวันกรณีเข้ำรักษำในสถำนพยำบำลในฐำนะคนไข้ใน
        Code PerPerson = Code.Create("PER_PERSON");
        Code PerDay = Code.Create("PER_DAY");
        Code CovAmountType = Code.Create("COVERAGE_AMOUNT");
        Code CovRangeType = Code.Create("COVERAGE_RANGE");
        Code CovLimitType = Code.Create("COVERAGE_LIMIT");

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
                .WithSymbol("วัน")
                .Build();
            context.Add(day);

            // Products
            var compulsory = Product.CreateBuilder(Guid.NewGuid(),
                Compulsory)
                .WithDescription("ประกันภัยคุ้มครองผู้ประสบภัยจำกรถ")
                .Build();

            context.Add(compulsory);

            // Coverage Types
            // BODILY_INJURY_OR_INJURY_TO_HEALTH
            var covType1 = CoverageType.CreateBuilder(Guid.NewGuid(), Cov001)
                .WithDescription("ความเสียหายต่อร่างกายหรืออนามัย")
                .Build();
            context.Add(covType1);
            var covType2 = CoverageType.CreateBuilder(Guid.NewGuid(), Cov002)
                .WithDescription("การเสียชีวิต หรือทุพพลภาพถาวรสิ้นเชิง")
                .Build();
            context.Add(covType2);
            var covType3 = CoverageType.CreateBuilder(Guid.NewGuid(), Cov003)
                .WithDescription("ทุพพลภาพอย่างถาวร หรือการสูญเสียอวัยวะ")
                .Build();
            context.Add(covType3);
            var covType4 = CoverageType.CreateBuilder(Guid.NewGuid(), Cov004)
                .WithDescription("ชดเชยรายวันกรณีเข้ารักษาในสถานพยาบาลในฐานะคนไข้ใน")
                .Build();
            context.Add(covType4);

            // Basises
            var perPersonBasis = new CoverageBasis(Guid.NewGuid(), PerPerson);
            perPersonBasis.UpdateDescription("ต่อหนึ่งคน");
            context.Add(perPersonBasis);
            var perDayBasis = new CoverageBasis(Guid.NewGuid(), PerDay);
            perDayBasis.UpdateDescription("ต่อวัน");
            context.Add(perDayBasis);

            // Level Benefit
            var covAmount = new CoverageLevelType(Guid.NewGuid(), CovAmountType);
            context.Add(covAmount);
            var covRange = new CoverageLevelType(Guid.NewGuid(), CovRangeType);
            context.Add(covRange);
            var covLimit = new CoverageLevelType(Guid.NewGuid(), CovLimitType);
            context.Add(covLimit);

            // Benefit Amount
            var covAmt1 = new CoverageAmount(Guid.NewGuid(), covAmount, perPersonBasis, bth, 80000);
            context.Add(covAmt1);
            var covAmt2 = new CoverageAmount(Guid.NewGuid(), covAmount, perPersonBasis, bth, 500000);
            context.Add(covAmt2);
            var covRange1 = new CoverageRange(Guid.NewGuid(), covRange, perPersonBasis, bth, 200000, 500000);
            context.Add(covRange1);
            var covAmt3 = new CoverageAmount(Guid.NewGuid(), covAmount, perDayBasis, bth, 200);
            context.Add(covAmt3);
            var covLimit1 = new CoverageLimit(Guid.NewGuid(), covLimit, perPersonBasis, day, 20);
            context.Add(covLimit1);

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

            addCovAvailAmount(context, comp, availRequiredType, Cov001.Value, 80000);
            addCovAvailAmount(context, comp, availRequiredType, Cov002.Value, 500000);
            addCovAvailRange(context, comp, availRequiredType, Cov003.Value, 200000, 500000);
            addCovAvailAmount(context, comp, availRequiredType, Cov004.Value, 200);
            addCovAvailLimit(context, comp, availRequiredType, Cov004.Value, 20);

            context.SaveChanges();
        }

        private void addCovAvailAmount(ProductsDbContext context, 
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
        private void addCovAvailRange(ProductsDbContext context,
            Product product,
            CoverageAvailabilityType covAvaiType,
            string covTypeCode,
            decimal limitFrom,
            decimal limitTo)
        {
            var cov1Type = context.CoverageTypes.Single(x => x.Code.Value == covTypeCode);
            var covLevel1 = context.CoverageLevels
                .OfType<CoverageRange>()
                .Single(x => x.CoverageLevelType.Code.Value == CovRangeType.Value
                && x.LimitFrom == limitFrom && x.LimitTo == limitTo);
            var requiredAvail = new CoverageAvailability(Guid.NewGuid(),
                product, covAvaiType, cov1Type, covLevel1);

            context.Add(requiredAvail);
        }
        private void addCovAvailLimit(ProductsDbContext context,
            Product product,
            CoverageAvailabilityType covAvaiType,
            string covTypeCode,
            decimal amount)
        {
            var cov1Type = context.CoverageTypes.Single(x => x.Code.Value == covTypeCode);
            var covLevel1 = context.CoverageLevels
                .OfType<CoverageLimit>()
                .Single(x => x.CoverageLevelType.Code.Value == CovLimitType.Value
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
            report += "Required Coverages\n";

            var grpCovTypes = requiredCoves.GroupBy(x => x.CoverageType.Code);
            foreach (var covAvais in grpCovTypes)
            {
                foreach (var covAvai in covAvais)
                {
                    var coverType = covAvai.CoverageType;
                    if (covAvai.CoverageLevel is CoverageAmount cavAmt)
                    {
                        report += $"- {cavAmt.Unit.Symbol}{cavAmt.Amount} {cavAmt.CoverageBasis.Description} สำหรับ{coverType.Description}\n";
                    }
                    if (covAvai.CoverageLevel is CoverageRange cavRange)
                    {
                        report += $"- {cavRange.Unit.Symbol}{cavRange.LimitFrom} ถึง {cavRange.Unit.Symbol}{cavRange.LimitTo} {cavRange.CoverageBasis.Description} สำหรับ{coverType.Description}\n";
                    }
                    if (covAvai.CoverageLevel is CoverageLimit cavLimit)
                    {
                        report += $"- รวมกันไม่เกิน {cavLimit.Amount} {cavLimit.Unit.Symbol} {cavLimit.CoverageBasis.Description} สำหรับ{coverType.Description}\n";
                    }
                }
                report += $"\n";

            }

            var sum = report;
        }
    }
}
