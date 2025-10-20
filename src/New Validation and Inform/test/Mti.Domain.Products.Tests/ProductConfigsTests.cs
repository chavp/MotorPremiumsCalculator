using Microsoft.EntityFrameworkCore;
using Mti.Domain.Products.Entities;
using Mti.Domain.Products.Tests.Models;
using Mti.Domain.Products.ValueObjects;
using Mti.Persistence.Products;
using Newtonsoft.Json.Linq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mti.Domain.Products.Tests
{
    public class ProductConfigsTests : BaseTests
    {
        [Fact]
        public void TestQueryByVehicleSize_สันดาบ()
        {

            using var context = _factory.CreateDbContext();

            var vehCombustion = context.VehicleFuelTypes.Single(x => x.Code == VehicleFuelType.Combustion);

            // ขนาดเครื่องยนต์ cc
            var cc = context.Units.Single(x => x.Code == Unit.CubicCentimeter);
            shouldBe_VehicleSize(context, vehCombustion, cc, "110", "120", "610", "620", "630", "730", "802");

            // จำนวนที่นั่ง seat
            var seat = context.Units.Single(x => x.Code == Unit.Seat);
            shouldBe_VehicleSize(context, vehCombustion, seat, "210", "220", "230");

            // น้ำหนักบรรทุก
            var kg = context.Units.Single(x => x.Code == Unit.Kilogram);
            shouldBe_VehicleSize(context, vehCombustion, kg, "320", "327", "340", "347", "420", "520", "540", "803", "804", "805");
        }

        [Fact]
        public void TestQueryByVehicleSize_ไฟฟ้า()
        {
            using var context = _factory.CreateDbContext();

            var vehElectric = context.VehicleFuelTypes.Single(x => x.Code == VehicleFuelType.Electric);

            var hp = context.Units.Single(x => x.Code == Unit.Horsepower);
            var seat = context.Units.Single(x => x.Code == Unit.Seat);
            var kg = context.Units.Single(x => x.Code == Unit.Kilogram);

            var vehUsage1 = context.VehicleUsages.Single(x => x.Code.Value == "1");
            var vehUsage2 = context.VehicleUsages.Single(x => x.Code.Value == "2");
            var vehUsage3 = context.VehicleUsages.Single(x => x.Code.Value == "3");
            var vehUsage4 = context.VehicleUsages.Single(x => x.Code.Value == "4");
            var vehUsage5 = context.VehicleUsages.Single(x => x.Code.Value == "5");
            var vehUsage6 = context.VehicleUsages.Single(x => x.Code.Value == "6");
            var vehUsageA = context.VehicleUsages.Single(x => x.Code.Value == "A");
            var vehUsageB = context.VehicleUsages.Single(x => x.Code.Value == "B");
            var vehUsageC = context.VehicleUsages.Single(x => x.Code.Value == "C");
            var vehUsageD = context.VehicleUsages.Single(x => x.Code.Value == "D");
            var vehUsageE = context.VehicleUsages.Single(x => x.Code.Value == "E");
            var vehUsageF = context.VehicleUsages.Single(x => x.Code.Value == "F");

            // (1) ลักษณะการใช้รถยนต์
            // 1.1 การใช้ส่วนบุคคล
            shouldBe_VehicleUsage(context, vehElectric, vehUsage1, "E11", "E21", "E61");

            // 1.2 การใช้เพื่อการพาณิชย์
            shouldBe_VehicleUsage(context, vehElectric, vehUsage2, "E12", "E22", "E32", "E42", "E52", "E62");

            // 1.3 การใช้รับจ้างสาธารณะ
            shouldBe_VehicleUsage(context, vehElectric, vehUsage3, "E23", "E63", "E73");

            // 1.4 การใช้เพื่อการพาณิชย์พิเศษ
            shouldBe_VehicleUsage(context, vehElectric, vehUsage4, "E34", "E54");

            // 1.5 การใช้ลากจูงรถพ่วงเพื่อการพาณิชย์
            shouldBe_VehicleUsage(context, vehElectric, vehUsage5, "E35");

            // 1.6 การใช้ลากจูงรถพ่วงเพื่อการพาณิชย์พิเศษ
            shouldBe_VehicleUsage(context, vehElectric, vehUsage6, "E36");

            // 1.7 รถยนต์ป้ายแดง สำหรับรถรหัส
            shouldBe_VehicleUsage(context, vehElectric, vehUsageA, "E8A");

            // 1.8 รถพยาบาล
            shouldBe_VehicleUsage(context, vehElectric, vehUsageB, "E8B");

            // 1.9 รถดับเพลิง
            shouldBe_VehicleUsage(context, vehElectric, vehUsageC, "E8C");

            // 1.10 รถใช้ในการเกษตร
            shouldBe_VehicleUsage(context, vehElectric, vehUsageD, "E8D");

            // 1.11 รถใช้ในการก่อสร้าง
            shouldBe_VehicleUsage(context, vehElectric, vehUsageE, "E8E");

            // 1.12 รถอื่น ๆ
            shouldBe_VehicleUsage(context, vehElectric, vehUsageF, "E8F");

            // (2) ขนาดรถยนต์
            // 2.1 ขนาดเครื่องยนต์
            shouldBe_VehicleSize(context, vehElectric, hp, "E11", "E12", "E61", "E62", "E63", "E73", "E8B");

            // 2.2 จำนวนที่นั่ง
            shouldBe_VehicleSize(context, vehElectric, seat, "E21", "E22", "E23");

            // 2.3 น้ำหนักบรรทุก
            shouldBe_VehicleSize(context, vehElectric, kg, "E32", "E34", "E35", "E36", "E42", "E52", "E54", "E8C", "E8D", "E8E");
        }

        [Fact]
        public void TestQueryVehicleWorkshopTypes()
        {

            using var context = _factory.CreateDbContext();

            var grage = context.VehicleWorkshopTypes.Where(x => x.LookupNames.Contains("อู่"))
                .SingleOrDefault();
            grage.ShouldNotBeNull();

            var dealer = context.VehicleWorkshopTypes.Where(x => x.LookupNames.Contains("ห้าง"))
                .SingleOrDefault();
            dealer.ShouldNotBeNull();
        }

        [Fact]
        public void TestProductConfigs()
        {
            var productsConfig = queryConfig<TB_M_CONFIG>(@"
                 SELECT Value1, Value3, Value11, Value4, Value5, Value6, Value7, Value8, Value9, Value12
                 from TB_M_CONFIG where Category = 'campaign' 
                 and CONVERT(DECIMAL(10,2), Value28) > 0 
                 AND YEAR([Start_Date]) BETWEEN YEAR(GETDATE())-1 AND YEAR(GETDATE())
                 AND [Start_Date] < End_Date
            ").OrderBy(a => Guid.NewGuid())
            .Where(x => x.Value3 == TestCampaign);
            using (var context = _factory.CreateDbContext())
            {
                foreach (var productConfig in productsConfig)
                {
                    var prd = context.Products
                        .Include(p => p.CoverageAvailabilities)
                            .ThenInclude(cv => cv.CoverageAvailabilityType)
                        .Include(p => p.CoverageAvailabilities)
                            .ThenInclude(cv => cv.CoverageType)
                        .Include(p => p.CoverageAvailabilities)
                            .ThenInclude(cv => cv.CoverageLevel)
                                .ThenInclude(cl => cl.CoverageLevelType)
                        .Include(p => p.CoverageAvailabilities)
                            .ThenInclude(cv => cv.CoverageLevel)
                                .ThenInclude(cl => cl.CoverageBasis)
                        .AsSplitQuery()
                        .Single(x => x.Code == productConfig.Value11);

                    var requiredCoverageAvailabilities = prd.CoverageAvailabilities
                        .Where(x => x.CoverageAvailabilityType.Code == CoverageAvailabilityType.Required)
                        .ToList();

                    // COV-001: Value4; 500000 บำท ต่อหนึ่งคน สำหรับกำรเสียชีวิต หรือทุพพลภำพถำวรสิ้นเชิง
                    shouldBeCoverage(requiredCoverageAvailabilities, "COV-001", productConfig.Value4);

                    // LCOV-001: Value5; ไม่เกินสิบล้ำนบำท สำหรับรถที่มีที่นั่งเกินเจ็ดคนหรือรถบรรทุกผู้โดยสำรรวมทั้งผู้ขับขี่เกินเจ็ดคน
                    shouldBeCoverage(requiredCoverageAvailabilities, "LCOV-001", productConfig.Value5);

                    // LCOV-002: Value6: รวมกันไม่เกินห้ำล้ำนบำทสำ หรับรถที่มีที่นั่งไม่เกินเจ็ดคนหรือรถบรรทุกผู้โดยสำรรวมทั้งผู้ขับขี่ไม่เกินเจ็ดคน
                    shouldBeCoverage(requiredCoverageAvailabilities, "LCOV-002", productConfig.Value6);

                    // COV-002: Value7: เสียชีวิต สูญเสียอวัยวะ ทุพพลภาพถาวรสิ้นเชิง ผู้ขับขี่ 1 บาท, ผู้โดยสาร บาท/คน
                    shouldBeCoverage(requiredCoverageAvailabilities, "COV-002", productConfig.Value7);

                    // COV-003: Value8: ค่ารักษาพยาบาล บาท/คน
                    shouldBeCoverage(requiredCoverageAvailabilities, "COV-003", productConfig.Value8);

                    // COV-004: Value9: การประกันตัวผู้ขับขี่ บาท/ครั้ง
                    shouldBeCoverage(requiredCoverageAvailabilities, "COV-004", productConfig.Value9);

                    // COV-SUMINS: Value12: ทุนประกันภัย
                    shouldBeCoverage(requiredCoverageAvailabilities, "COV-SUMINS", productConfig.Value12);
                }
            }
        }

        [Fact]
        public void TestSeed()
        {
            using (var context = _factory.CreateDbContext())
            using (var tran = context.Database.BeginTransaction())
            {
                seed(context, TestCampaign);
                tran.Commit();
            }
        }

        public void seed(ProductsDbContext context, string campaignCode)
        {
            // VOLUNTARY
            var configPackages = queryConfig<TB_M_CONFIG>(@"
select 
  DISTINCT [Value3]
  ,[Value11]
 from TB_M_CONFIG where Category = 'campaign' and CONVERT(DECIMAL(10,2), Value28) > 0
 AND YEAR([Start_Date]) BETWEEN YEAR(GETDATE())-1 AND YEAR(GETDATE())
 order by [Value3], [Value11] DESC
")
            .Where(x => x.Value3 == campaignCode)
            .ToList();

            var selectableProductFeatureAvailabilityType = context
                .ProductFeatureAvailabilityTypes.Single(x => x.Code == ProductFeatureAvailabilityType.Selectable);

            foreach (var config in configPackages)
            {
                var campaign = context.Campaigns
                    .Include(c => c.Products)
                        .ThenInclude(p => p.ProductFeatureAvailabilities)
                            .ThenInclude(pf => pf.ProductFeature)
                    .Single(x => x.Code == config.Value3);

                var package = campaign
                    .Products
                    .SingleOrDefault(x => x.Code == config.Value11);

                // clear available brands
                var availableBrands = package.ProductFeatureAvailabilities
                    .Where(pfa => pfa.ProductFeature is VehicleBrandFeature)
                    .ToList();
                if(availableBrands.Any())
                {
                    context.RemoveRange(availableBrands);
                    context.SaveChanges();
                }
                // clear available models
                var availableModels = package.ProductFeatureAvailabilities
                    .Where(pfa => pfa.ProductFeature is VehicleModelFeature)
                    .ToList();
                if (availableModels.Any())
                {
                    context.RemoveRange(availableModels);
                    context.SaveChanges();
                }

                var brandsConfigs = queryConfig<TB_M_CONFIG>(@"
 select 
  DISTINCT [Value15], [Value16]
 from TB_M_CONFIG where Category = 'campaign' and CONVERT(DECIMAL(10,2), Value28) > 0
 AND YEAR([Start_Date]) BETWEEN YEAR(GETDATE())-1 AND YEAR(GETDATE())
AND [Value3] = @Value3 AND [Value11] = @Value11
"
                    , new
                    {
                        Value3 = campaign.Code.Value,
                        Value11 = package.Code.Value
                    }).ToList();

                var allBrands = new List<string>();
                var allModels = new List<string>();
                foreach (var item in brandsConfigs)
                {
                    allBrands.AddRange(item.Value15.Split(",", StringSplitOptions.RemoveEmptyEntries));
                    allModels.AddRange(item.Value16.Split(",", StringSplitOptions.RemoveEmptyEntries));
                }

                allBrands = allBrands.Distinct().ToList();
                allModels = allModels.Distinct().ToList();

                var loadAllBrands = context.ProductFeatures.OfType<VehicleBrandFeature>()
                        .Include(x => x.Models)
                        .Where(x => allBrands.Contains(x.Code))
                        .ToList();

                // add brands
                foreach (var brand in allBrands)
                {
                    var vehBrand = loadAllBrands
                        .Single(x => x.Code == brand);
                    var prdFeatureAvai = ProductFeatureAvailability.CreateBuilder(
                        package, selectableProductFeatureAvailabilityType, vehBrand)
                        .Build();
                    context.Add(prdFeatureAvai);
                    context.SaveChanges();

                    // add models
                    foreach (var model in allModels)
                    {
                        var vehModel = vehBrand.Models
                            .SingleOrDefault(x => x.Code == model);
                        if (vehModel == null) continue;

                        var prdModelFeatureAvai = ProductFeatureAvailability.CreateBuilder(
                            package, selectableProductFeatureAvailabilityType, vehModel)
                            .Build();
                        context.Add(prdModelFeatureAvai);
                        context.SaveChanges();
                    }
                }
            }
        }

    }
}