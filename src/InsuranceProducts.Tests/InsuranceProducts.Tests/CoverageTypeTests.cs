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
        Code Cov005 = Code.Create("COV005"); // จำนวนเงินคุ้มครองสูงสุด (1), (2), (3) และ (4)
        Code PerPerson = Code.Create("PER_PERSON");
        Code PerDay = Code.Create("PER_DAY");
        Code PerCase = Code.Create("PER_CASE");

        Code PerAccidentVehicleLowSeat = Code.Create("PER_ACCIDENT_VEHICLE_LOW_SEAT");
        Code PerAccidentVehicleHightSeat = Code.Create("PER_ACCIDENT_VEHICLE_HIGHT_SEAT");

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
            var covType1 = CoverageType.CreateBuilder(Cov001)
                .WithDescription("ความเสียหายต่อร่างกายหรืออนามัย")
                .Build();
            context.Add(covType1);
            var covType2 = CoverageType.CreateBuilder(Cov002)
                .WithDescription("การเสียชีวิต หรือทุพพลภาพถาวรสิ้นเชิง")
                .Build();
            context.Add(covType2);
            var covType3 = CoverageType.CreateBuilder(Cov003)
                .WithDescription("ทุพพลภาพอย่างถาวร หรือการสูญเสียอวัยวะ")
                .Build();
            context.Add(covType3);
            var covType4 = CoverageType.CreateBuilder(Cov004)
                .WithDescription("ชดเชยรายวันกรณีเข้ารักษาในสถานพยาบาลในฐานะคนไข้ใน")
                .Build();
            context.Add(covType4);
            var covType5 = CoverageType.CreateBuilder(Cov005)
                .WithDescription("จำนวนเงินคุ้มครองสูงสุด (1), (2), (3) และ (4)")
                .Build();
            context.Add(covType5);

            context.Add(new CoverageTypeComposition(covType5, covType1));
            context.Add(new CoverageTypeComposition(covType5, covType2));
            context.Add(new CoverageTypeComposition(covType5, covType3));
            context.Add(new CoverageTypeComposition(covType5, covType4));
            //covType5.AddToComposition(covType1);
            //covType5.AddToComposition(covType2);
            //covType5.AddToComposition(covType3);
            //covType5.AddToComposition(covType4);

            // Basises
            var perPersonBasis = new CoverageBasis(Guid.NewGuid(), PerPerson);
            perPersonBasis.UpdateDescription("ต่อหนึ่งคน");
            context.Add(perPersonBasis);
            var perDayBasis = new CoverageBasis(Guid.NewGuid(), PerDay);
            perDayBasis.UpdateDescription("ต่อวัน");
            context.Add(perDayBasis);
            var perCaseBasis = new CoverageBasis(Guid.NewGuid(), PerCase);
            perCaseBasis.UpdateDescription("ต่อกรณี");
            context.Add(perCaseBasis);
            var perAccidentVehicleLowSeatBasis = new CoverageBasis(Guid.NewGuid(), PerAccidentVehicleLowSeat);
            perAccidentVehicleLowSeatBasis.UpdateDescription("ที่นั่งไม่เกินเจ็ดคนหรือรถบรรทุกผู้โดยสารรวมทั้งผู้ขับขี่ไม่เกินเจ็ดคน ต่ออุบัติเหตุ");
            context.Add(perAccidentVehicleLowSeatBasis);
            var perAccidentVehicleHightSeatBasis = new CoverageBasis(Guid.NewGuid(), PerAccidentVehicleHightSeat);
            perAccidentVehicleHightSeatBasis.UpdateDescription("ที่นั่งเกินเจ็ดคนหรือรถบรรทุกผู้โดยสารรวมทั้งผู้ขับขี่เกินเจ็ดคน ต่ออุบัติเหตุ");
            context.Add(perAccidentVehicleHightSeatBasis);

            // Level Benefit
            var covAmount = CoverageLevelType.CreateBuilder(CovAmountType).Build();
            context.Add(covAmount);
            var covRange = CoverageLevelType.CreateBuilder(CovRangeType).Build();
            context.Add(covRange);
            var covLimit = CoverageLevelType.CreateBuilder(CovLimitType).Build();
            context.Add(covLimit);

            // Benefit Amount
            var covLevelBuilder = CoverageLevel.CreateBuilder();
            var covAmt1 = covLevelBuilder
                            .WithCoverageLevelType(covAmount)
                            .WithAmount(80000)
                            .WithUnit(bth)
                            .WithCoverageBasis(perPersonBasis)
                            .Build();
            context.Add(covAmt1);
            var covAmt2 = covLevelBuilder
                            .WithCoverageLevelType(covAmount)
                            .WithAmount(500000)
                            .WithUnit(bth)
                            .WithCoverageBasis(perPersonBasis)
                            .Build();
            context.Add(covAmt2);
            var covRange1 = covLevelBuilder
                            .WithCoverageLevelType(covRange)
                            .WithRange(200000, 500000)
                            .WithUnit(bth)
                            .WithCoverageBasis(perPersonBasis)
                            .Build();
            context.Add(covRange1);
            var covAmt3 = covLevelBuilder
                            .WithCoverageLevelType(covAmount)
                            .WithAmount(200)
                            .WithUnit(bth)
                            .WithCoverageBasis(perPersonBasis)
                            .Build();
            context.Add(covAmt3);
            var covLimit1 = covLevelBuilder
                            .WithCoverageLevelType(covLimit)
                            .WithLimit(20)
                            .WithUnit(day)
                            .WithCoverageBasis(perCaseBasis)
                            .Build();
            context.Add(covLimit1);

            var covComLimit2 = covLevelBuilder
                            .WithCoverageLevelType(covLimit)
                            .WithLimit(504000)
                            .WithUnit(bth)
                            .WithCoverageBasis(perPersonBasis)
                            .Build();
            context.Add(covComLimit2);
            var covComLimit3 = covLevelBuilder
                           .WithCoverageLevelType(covLimit)
                           .WithLimit(5000000)
                           .WithUnit(bth)
                           .WithCoverageBasis(perAccidentVehicleLowSeatBasis)
                           .Build();
            context.Add(covComLimit3);
            var covComLimit4 = covLevelBuilder
                           .WithCoverageLevelType(covLimit)
                           .WithLimit(10000000)
                           .WithUnit(bth)
                           .WithCoverageBasis(perAccidentVehicleHightSeatBasis)
                           .Build();
            context.Add(covComLimit4);

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

            addCovAvailLimit(context, comp, availRequiredType, Cov005.Value, 504000);
            addCovAvailLimit(context, comp, availRequiredType, Cov005.Value, 5000000);
            addCovAvailLimit(context, comp, availRequiredType, Cov005.Value, 10000000);

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
                && x.MinimumAmount == limitFrom && x.MaximumAmount == limitTo);
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
                        .ThenInclude(z => z.ToCompositions)
                .Include(x => x.CoverageAvailabilities)
                    .ThenInclude(y => y.CoverageLevel)
                        .ThenInclude(z => z.CoverageLevelType)
                .Include(x => x.CoverageAvailabilities)
                    .ThenInclude(y => y.CoverageLevel)
                        .ThenInclude(z => z.CoverageBasis)
                .Include(x => x.CoverageAvailabilities)
                    .ThenInclude(y => y.CoverageLevel)
                        .ThenInclude(z => z.Unit)
                            .ThenInclude(zz => zz.UnitCategory)
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
                        report += $"- {cavRange.Unit.Symbol}{cavRange.MinimumAmount} ถึง {cavRange.Unit.Symbol}{cavRange.MaximumAmount} {cavRange.CoverageBasis.Description} สำหรับ{coverType.Description}\n";
                    }
                    if (covAvai.CoverageLevel is CoverageLimit cavLimit)
                    {
                        var disAmount = $"{cavLimit.Amount} {cavLimit.Unit.Symbol}";
                        if(cavLimit.Unit.UnitCategory.Code == UnitCategory.Currency)
                            disAmount = $"{cavLimit.Unit.Symbol}{cavLimit.Amount}";
                        report += $"- รวมกันไม่เกิน {disAmount} {cavLimit.CoverageBasis.Description} สำหรับ{coverType.Description}\n";
                    }
                    if (covAvai.Id == covAvais.Last().Id)
                    {
                        var index = 1;
                        foreach (var toCovType in coverType.ToCompositions)
                        {
                            report += $"\t({index++}). {toCovType.ToCoverageType.Description}\n";
                        }
                    }
                }
                report += $"\n";

            }

            var sum = report;
        }
    }
}
