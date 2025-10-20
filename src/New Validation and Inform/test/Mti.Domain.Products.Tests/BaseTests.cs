using Dapper;
using Microsoft.EntityFrameworkCore;
using Mti.Domain.Products.Entities;
using Mti.Domain.Products.Tests.Models;
using Mti.Domain.Products.ValueObjects;
using Mti.Persistence.Products;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mti.Domain.Products.Tests
{
    public abstract class BaseTests
    {
        // docker run --name insurance-db -e POSTGRES_USER=admin -e POSTGRES_PASSWORD=admin123 -d -p 5432:5432 postgres

        protected string _con = "Host=localhost;Port=5432;Database=insurance-db;Username=admin;Password=admin123";
        protected string _conSource = "Data Source=MTIDDDB02;Initial Catalog=NewValidateDB;TrustServerCertificate=True;User ID=nvld_rwuser;Password=$nvLd@57489;";

        protected ProductsDbContextFactory _factory;

        Code PerPerson = Code.Create("PER_PERSON");
        Code PerTime = Code.Create("PER_TIME");
        Code PerPolicy = Code.Create("PER_POLICY");
        Code PerAccidentVehicleLowSeat = Code.Create("PER_ACCIDENT_VEHICLE_LOW_SEAT");
        Code PerAccidentVehicleHightSeat = Code.Create("PER_ACCIDENT_VEHICLE_HIGHT_SEAT");

        Code CovAmountType = Code.Create("COVERAGE_AMOUNT");
        Code CovRangeType = Code.Create("COVERAGE_RANGE");
        Code CovLimitType = Code.Create("COVERAGE_LIMIT");

        protected string TestCampaign = "2CARE0";
        public BaseTests()
        {
            _factory = new ProductsDbContextFactory(_con);

            lock (LockSeedData)
            {
                // ProductConfigsTests
                seedProducts_Units();
                seedProducts_VehicleUsages_สันดาบ();
                seedProducts_VehicleTypeVoluntary_สันดาป();
                seedProducts_VehicleUsages_ไฟฟ้า();
                seedProducts_VehicleTypeVoluntary_ไฟฟ้า();
                seedProducts_VehicleWorkshopTypes();
                seedProducts_ProductFeatureTypes();

                seedProducts_VehicleVoluntaryFeatures();
                seedProducts_VehicleCompulsoryFeatures();

                seedProducts_PolicyTypes();
                seedProducts_Campaigns();
                seedProducts_CompulsoryPackages();
                seedProducts_MasterCoverages();
                seedProducts_CoverageAvailabilities();
                seedProducts_MasterProductFeatures();
                seedProducts_VehicleCompulsoryFeature_ProductFeatureAvailabilities();
                seedProducts_MasterProductPremiums();
                seedProducts_VehicleCompulsoryFeature_ProductFeatureAvailabilities_Premiums();

                seedProducts_VehiclePriceGroupFeatures();
                seedProducts_VehicleBrandModels();

                seedProducts_VoluntaryCampaignPackages(TestCampaign);
                seedProducts_VehicleVoluntaryFeature_ProductFeatureAvailabilities(TestCampaign);
                seedProducts_VehicleVoluntaryFeature_ProductFeatureAvailabilities_Premiums(TestCampaign);


            }
        }

        private static object LockSeedData = new object();

        protected void applyMigration(string migrationId, string productVersion,
            Action<ProductsDbContext> action)
        {
            using (var ctx = _factory.CreateDbContext())
            using (var tran = ctx.Database.BeginTransaction())
            {
                var con = ctx.Database.GetDbConnection();
                var migrate = con
                    .Query<string>(
                    @"SELECT ""MigrationId"" FROM ""__EFMigrationsHistory""  WHERE ""MigrationId"" = @MigrationId",
                    new { MigrationId = migrationId })
                    .SingleOrDefault();
                if (migrate != null) return;

                action(ctx);

                con.Execute(@"
INSERT INTO ""__EFMigrationsHistory"" (""MigrationId"", ""ProductVersion"")
VALUES (@MigrationId, @ProductVersion);
", new { MigrationId = migrationId, ProductVersion = productVersion });

                tran.Commit();
            }
        }


        protected List<T> queryConfig<T>(string query, object? para = null)
        {
            var configs = new List<T>();
            using (var con = new SqlConnection(_conSource))
            {
                configs = con.Query<T>(query, para).ToList();
            }
            return configs;
        }

        protected void seedProducts_PolicyTypes()
        {
            applyMigration("seedProducts_PolicyTypes", "1.0.0", context =>
            {
                var configs = queryConfig<TB_M_CONFIG>(@"
SELECT [Code]
      ,[Description]
  FROM [NewValidateDB].[dbo].[TB_M_CONFIG]
  WHERE [Category] = 'POLICYTYPE'
  ORDER BY [Code]
");
                foreach (var config in configs)
                {
                    var target = context.PolicyTypes.SingleOrDefault(x => x.Code == config.Code);
                    if (target == null)
                    {
                        target = PolicyType
                            .CreateBuilder(config.Code, config.Description)
                            .Build();
                        context.Add(target);
                    }
                }

                context.SaveChanges();
            });
        }
        protected void seedProducts_Campaigns()
        {
            applyMigration("seedProducts_Campaigns", "1.0.0", context =>
            {
                // VOLUNTARY
                var configs = queryConfig<TB_M_CONFIG>(@"
select 
  DISTINCT [Value1] , [Value3]
 from TB_M_CONFIG where Category = 'campaign' and CONVERT(DECIMAL(10,2), Value28) > 0
 AND YEAR([Start_Date]) BETWEEN YEAR(GETDATE())-1 AND YEAR(GETDATE())
 order by [Value1], [Value3]
");
                // select descriptions
                foreach (var config in configs)
                {
                    var descs = queryConfig<string>(@"
                        select 
                          [Description]
                         from TB_M_CONFIG where Category = 'campaign' and CONVERT(DECIMAL(10,2), Value28) > 0
                         AND YEAR([Start_Date]) BETWEEN YEAR(GETDATE())-1 AND YEAR(GETDATE())
                        AND Value1 = @Value1 AND Value3 = @Value3
                        ", new { Category = "campaign", Value1 = config.Value1, Value3 = config.Value3 })
                        .Distinct()
                        .ToList();

                    config.Description = string.Join(",", descs);
                }

                foreach (var config in configs)
                {
                    var poType = context.PolicyTypes.Single(x => x.Code == config.Value1);
                    var target = context.Campaigns.SingleOrDefault(x => x.PolicyType == poType && x.Code == config.Value3);
                    if (target == null)
                    {
                        target = Campaign
                            .CreateBuilder(poType, config.Value3, config.Value3)
                            .WithDescription(config.Description)
                            .Build();
                        context.Add(target);
                    }
                }
                context.SaveChanges();

                // COMPULSORY
                configs = queryConfig<TB_M_CONFIG>(@"
                select 
                  DISTINCT 
                  [Value11]
                 from TB_M_CONFIG where
                 VAlue1 = 'CTP' and Category = 'campaign' and Sub_Category = 'COMMON' 
                and CONVERT(DECIMAL(10,2), Value28) > 0
                 order by Value11 DESC
                ");


                foreach (var config in configs)
                {
                    var poType = context.PolicyTypes.Single(x => x.Code == "CTP");
                    var target = context.Campaigns.SingleOrDefault(x => x.PolicyType == poType
                    && x.Code == config.Value11);
                    if (target == null)
                    {
                        target = Campaign
                            .CreateBuilder(poType, config.Value11, config.Value11)
                            .Build();
                        context.Add(target);
                    }
                }
                context.SaveChanges();
            });
        }
        protected void seedProducts_VoluntaryCampaignPackages(string campaignCode)
        {
            applyMigration($"seedProducts_VoluntaryCampaignPackages_{campaignCode}", "1.0.0", context =>
            {
                // VOLUNTARY
                var configPackages = queryConfig<TB_M_CONFIG>(@"
select 
  DISTINCT [Value3]
  ,[Value11]
 ,[Remark]
 --,value34 as base_premium
 --,CONVERT(DECIMAL(10,2), value25) as net_premium
 --,[Start_Date]
 --,[End_Date]
 from TB_M_CONFIG where Category = 'campaign' and CONVERT(DECIMAL(10,2), Value28) > 0
 AND YEAR([Start_Date]) BETWEEN YEAR(GETDATE())-1 AND YEAR(GETDATE())
 order by [Value3], [Value11], [Remark] DESC
")
                .Where(x => x.Value3 == campaignCode)
                .ToList();

                //var dup = configPackages.GroupBy(x => new { x.Value3, x.Value11, x.Remark })
                //    .Where(x => x.Count() > 1).ToList();
                foreach (var config in configPackages)
                {
                    var campaign = context
                        .Campaigns
                        .Include(c => c.Products)
                        .Single(x => x.Code == config.Value3);
                    var target = campaign
                        .Products
                        .SingleOrDefault(x => x.Code == config.Value11);
                    if (target == null)
                    {
                        target = Product.CreateBuilder(config.Value11, config.Value11)
                            .WithDescription(config.Remark)
                            .Build();
                        context.Add(target);
                    }

                    if (!target.Campaigns.Any(x => x == campaign))
                    {
                        target.Campaigns.Add(campaign);
                    }

                    // update sale start-end date
                    var maxStartDate = queryConfig<TB_M_CONFIG>(
                        @"
 select 
 MIN([Start_Date]) as Start_Date,
 MAX([End_Date]) as End_Date
 from TB_M_CONFIG where Category = 'campaign' and CONVERT(DECIMAL(10,2), Value28) > 0
 AND YEAR([Start_Date]) BETWEEN YEAR(GETDATE())-1 AND YEAR(GETDATE())
AND [Value3] = @Value3 AND [Value11] = @Value11
", new { Value3 = config.Value3, Value11 = config.Value11 }
                        ).Single();

                    target.UpdateSaleStartDate(DateOnly.FromDateTime(maxStartDate.Start_Date));
                    target.UpdateSaleEndDate(DateOnly.FromDateTime(maxStartDate.End_Date));

                    context.SaveChanges();
                }
            });
        }
        protected void seedProducts_CompulsoryPackages()
        {
            applyMigration("seedProducts_CompulsoryPackages", "1.0.0", context =>
            {
                // VOLUNTARY
                var configPackages = queryConfig<TB_M_CONFIG>(@"
 select 
  DISTINCT 
  [Value11]
  ,[Value10]
 ,[Description]
 --,value34 as base_premium
 ,CONVERT(DECIMAL(10,2), value25) as net_premium
 ,[Start_Date]
 ,[End_Date]
 from TB_M_CONFIG where
 VAlue1 = 'CTP' and Category = 'campaign' and Sub_Category = 'COMMON' and CONVERT(DECIMAL(10,2), Value28) > 0
 order by Value11, [Start_Date] DESC
");

                //var dup = configPackages.GroupBy(x => new { x.Value3, x.Value11, x.Remark })
                //    .Where(x => x.Count() > 1).ToList();
                foreach (var config in configPackages)
                {
                    var campaign = context.Campaigns.Single(x => x.Code == config.Value11);
                    var target = context
                        .Products
                        .Include(p => p.Campaigns)
                        .SingleOrDefault(x => x.Code == config.Value10);
                    if (target == null)
                    {
                        target = Product.CreateBuilder(config.Value10, config.Value10)
                            .WithDescription(config.Description)
                            .WithSaleStartDate(DateOnly.FromDateTime(config.Start_Date))
                            .WithSaleEndDate(DateOnly.FromDateTime(config.End_Date))
                            .Build();
                        context.Add(target);
                    }

                    if (!target.Campaigns.Any(x => x == campaign))
                    {
                        target.Campaigns.Add(campaign);
                    }

                    context.SaveChanges();
                }
            });
        }

        protected void seedProducts_MasterCoverages()
        {
            applyMigration("seedProducts_MasterCoverages", "1.0.0", context =>
            {
                // Coverage Availability Type
                var requiredAvailabilityType = CoverageAvailabilityType.CreateBuilder(
                    Code.Create(CoverageAvailabilityType.Required))
                    .WithDescription("กำหนดความคุ้มครองตามกฏกมายกำหนด")
                    .Build();
                context.Add(requiredAvailabilityType);

                var covType1 = CoverageType.CreateBuilder(Code.Create("COV-001"))
                    .WithDescription("กำรเสียชีวิต หรือทุพพลภำพถำวรสิ้นเชิง")
                    .Build();
                context.Add(covType1);

                var covType2 = CoverageType.CreateBuilder(Code.Create("LCOV-001"))
                    .WithDescription("รถที่มีที่นั่งเกินเจ็ดคนหรือรถบรรทุกผู้โดยสำรรวมทั้งผู้ขับขี่เกินเจ็ดคน")
                    .Build();
                context.Add(covType2);

                var covType3 = CoverageType.CreateBuilder(Code.Create("LCOV-002"))
                    .WithDescription("รถที่มีที่นั่งไม่เกินเจ็ดคนหรือรถบรรทุกผู้โดยสำรรวมทั้งผู้ขับขี่ไม่เกินเจ็ดคน")
                    .Build();
                context.Add(covType3);

                var covType4 = CoverageType.CreateBuilder(Code.Create("COV-002"))
                    .WithDescription("เสียชีวิต สูญเสียอวัยวะ ทุพพลภาพถาวรสิ้นเชิง")
                    .Build();
                context.Add(covType4);

                var covType5 = CoverageType.CreateBuilder(Code.Create("COV-003"))
                    .WithDescription("ค่ารักษาพยาบาล")
                    .Build();
                context.Add(covType5);

                var covType6 = CoverageType.CreateBuilder(Code.Create("COV-004"))
                    .WithDescription("การประกันตัวผู้ขับขี่")
                    .Build();
                context.Add(covType6);

                var covSumIns = CoverageType.CreateBuilder(Code.Create("COV-SUMINS"))
                    .WithDescription("ทุนประกันภัย")
                    .Build();
                context.Add(covSumIns);

                // Basises
                context.Add(CoverageBasis.CreateBuilder(Guid.NewGuid(), PerPerson).WithDescription("ต่อหนึ่งคน").Build());
                context.Add(CoverageBasis.CreateBuilder(Guid.NewGuid(), PerTime).WithDescription("ต่อครั้ง").Build());
                context.Add(CoverageBasis.CreateBuilder(Guid.NewGuid(), PerPolicy).WithDescription("ต่อกรมธรรม์ประกันภัย").Build());
                context.Add(CoverageBasis.CreateBuilder(Guid.NewGuid(), PerAccidentVehicleLowSeat).WithDescription("ที่นั่งไม่เกินเจ็ดคนหรือรถบรรทุกผู้โดยสารรวมทั้งผู้ขับขี่ไม่เกินเจ็ดคน ต่ออุบัติเหตุ").Build());
                context.Add(CoverageBasis.CreateBuilder(Guid.NewGuid(), PerAccidentVehicleHightSeat).WithDescription("ที่นั่งเกินเจ็ดคนหรือรถบรรทุกผู้โดยสารรวมทั้งผู้ขับขี่เกินเจ็ดคน ต่ออุบัติเหตุ").Build());

                // Level Benefit
                var covAmount = CoverageLevelType.CreateBuilder(CovAmountType).Build();
                context.Add(covAmount);
                var covRange = CoverageLevelType.CreateBuilder(CovRangeType).Build();
                context.Add(covRange);
                var covLimit = CoverageLevelType.CreateBuilder(CovLimitType).Build();
                context.Add(covLimit);

                context.SaveChanges();
            });
        }
        protected void seedProducts_CoverageAvailabilities()
        {
            applyMigration("seedProducts_CoverageAvailabilities", "1.0.0", context =>
            {
                // COV-001: Value4; 500000 บำท ต่อหนึ่งคน สำหรับกำรเสียชีวิต หรือทุพพลภำพถำวรสิ้นเชิง
                // LCOV-001: Value5; ไม่เกินสิบล้ำนบำท สำหรับรถที่มีที่นั่งเกินเจ็ดคนหรือรถบรรทุกผู้โดยสำรรวมทั้งผู้ขับขี่เกินเจ็ดคน
                // LCOV-002: Value6: รวมกันไม่เกินห้ำล้ำนบำทสำ หรับรถที่มีที่นั่งไม่เกินเจ็ดคนหรือรถบรรทุกผู้โดยสำรรวมทั้งผู้ขับขี่ไม่เกินเจ็ดคน
                // COV-002: Value7: เสียชีวิต สูญเสียอวัยวะ ทุพพลภาพถาวรสิ้นเชิง ผู้ขับขี่ 1 บาท, ผู้โดยสาร บาท/คน
                // COV-003: Value8: ค่ารักษาพยาบาล บาท/คน
                // COV-004: Value9: การประกันตัวผู้ขับขี่ บาท/ครั้ง
                // COV-SUMINS: Value12: ทุนประกันภัย
                var configCoverages = queryConfig<TB_M_CONFIG>(@"
 SELECT DISTINCT Value3, Value11, Value4, Value5, Value6, Value7, Value8, Value9, Value12
 from TB_M_CONFIG where Category = 'campaign' 
 and CONVERT(DECIMAL(10,2), Value28) > 0 
 AND YEAR([Start_Date]) BETWEEN YEAR(GETDATE())-1 AND YEAR(GETDATE())
 AND [Start_Date] < End_Date
");

                var baht = context.Units.Single(x => x.Code == Unit.Baht);

                var perPerson = context.CoverageBasises.Single(x => x.Code == PerPerson);
                var perTime = context.CoverageBasises.Single(x => x.Code == PerTime);
                var perPolicy = context.CoverageBasises.Single(x => x.Code == PerPolicy);
                var perAccidentVehicleLowSeat = context.CoverageBasises.Single(x => x.Code == PerAccidentVehicleLowSeat);
                var perAccidentVehicleHightSeat = context.CoverageBasises.Single(x => x.Code == PerAccidentVehicleHightSeat);

                // 1) 1.1 กำรเสียชีวิต หรือทุพพลภำพถำวรสิ้นเชิง
                var covTypeValue4 = context.CoverageTypes.Single(x => x.Code == "COV-001");

                // (5) รถที่มีที่นั่งเกินเจ็ดคนหรือรถบรรทุกผู้โดยสำรรวมทั้งผู้ขับขี่เกินเจ็ดคน จำนวนเงินคุ้มครองสูงสุดสำ หรับ (1) (2) (3) และ (4)
                var covTypeValue5 = context.CoverageTypes.Single(x => x.Code == "LCOV-001");

                // (5) รถที่มีที่นั่งไม่เกินเจ็ดคนหรือรถบรรทุกผู้โดยสำรรวมทั้งผู้ขับขี่ไม่เกินเจ็ดคน จำนวนเงินคุ้มครองสูงสุดสำ หรับ (1) (2) (3) และ (4)
                var covTypeValue6 = context.CoverageTypes.Single(x => x.Code == "LCOV-002");

                // เสียชีวิต สูญเสียอวัยวะ ทุพพลภาพถาวรสิ้นเชิง
                var covTypeValue7 = context.CoverageTypes.Single(x => x.Code == "COV-002");

                // 2) ค่ารักษาพยาบาล
                var covTypeValue8 = context.CoverageTypes.Single(x => x.Code == "COV-003");

                // 3) การประกันตัวผู้ขับขี่
                var covTypeValue9 = context.CoverageTypes.Single(x => x.Code == "COV-004");

                // ทุนประกันภัย
                var covSumInsValue12 = context.CoverageTypes.Single(x => x.Code == "COV-SUMINS");

                var availRequiredType = context
                    .CoverageAvailabilityTypes.Single(x => x.Code == CoverageAvailabilityType.Required);

                //var prds = context.Products.ToImmutableList();

                foreach (var configCoverage in configCoverages)
                {
                    var campaign = context.Campaigns
                        .Include(c => c.Products)
                        .Single(x => x.Code == configCoverage.Value3);
                    var prd = campaign
                        .Products
                        .SingleOrDefault(x => x.Code == configCoverage.Value11);

                    if (prd == null) continue;

                    // Value4; 500000 บำท ต่อหนึ่งคน สำหรับกำรเสียชีวิต หรือทุพพลภำพถำวรสิ้นเชิง
                    addCovAvailAmount(context, prd,
                        availRequiredType,
                        "COV-001",
                        configCoverage.Value4,
                        baht,
                        perPerson);

                    // Value5; ไม่เกินสิบล้ำนบำท สำหรับรถที่มีที่นั่งเกินเจ็ดคนหรือรถบรรทุกผู้โดยสำรรวมทั้งผู้ขับขี่เกินเจ็ดคน
                    addCovAvailLimit(context, prd,
                        availRequiredType,
                        "LCOV-001",
                        configCoverage.Value5,
                        baht,
                        perAccidentVehicleHightSeat);

                    // Value6: รวมกันไม่เกินห้ำล้ำนบำทสำ หรับรถที่มีที่นั่งไม่เกินเจ็ดคนหรือรถบรรทุกผู้โดยสำรรวมทั้งผู้ขับขี่ไม่เกินเจ็ดคน
                    addCovAvailLimit(context, prd,
                        availRequiredType,
                        "LCOV-002",
                        configCoverage.Value6,
                        baht,
                        perAccidentVehicleLowSeat);

                    // Value7: เสียชีวิต สูญเสียอวัยวะ ทุพพลภาพถาวรสิ้นเชิง ผู้ขับขี่ 1 บาท, ผู้โดยสาร บาท/คน
                    addCovAvailAmount(context, prd,
                        availRequiredType,
                        "COV-002",
                        configCoverage.Value7,
                        baht,
                        perPerson);

                    // Value8: ค่ารักษาพยาบาล บาท/คน
                    addCovAvailAmount(context, prd,
                        availRequiredType,
                        "COV-003",
                        configCoverage.Value8,
                        baht,
                        perPerson);

                    // Value9: การประกันตัวผู้ขับขี่ บาท/ครั้ง
                    addCovAvailAmount(context, prd,
                        availRequiredType,
                        "COV-004",
                        configCoverage.Value9,
                        baht,
                        perTime);

                    // Value12: ทุนประกันภัย
                    addCovAvailAmount(context, prd,
                        availRequiredType,
                        "COV-SUMINS",
                        configCoverage.Value12,
                        baht,
                        perPolicy);

                    context.SaveChanges();
                }
            });
        }

        protected void seedProducts_MasterProductFeatures()
        {
            applyMigration("seedProducts_MasterProductFeatures", "1.0.0", context =>
            {
                // Coverage Availability Type
                var standardAvailabilityType = ProductFeatureAvailabilityType.CreateBuilder(
                    Code.Create(ProductFeatureAvailabilityType.Standard))
                    .WithDescription("กำหนดไว้เฉพาะ")
                    .Build();
                context.Add(standardAvailabilityType);

                var selectableAvailabilityType = ProductFeatureAvailabilityType.CreateBuilder(
                    Code.Create(ProductFeatureAvailabilityType.Selectable))
                    .WithDescription("เลือกได้")
                    .Build();
                context.Add(selectableAvailabilityType);

                context.SaveChanges();
            });
        }
        protected void seedProducts_VehicleVoluntaryFeature_ProductFeatureAvailabilities(string campaign)
        {
            applyMigration(
                $"seedProducts_VehicleVoluntaryFeature_ProductFeatureAvailabilities_{campaign}", "1.0.0", context =>
            {
                var vehicleVoluntaryPackagesConfig = queryConfig<TB_M_CONFIG>(@"
select 
  DISTINCT [Value3]
  ,[Value11]
 ,Value10
 ,Value14
 from TB_M_CONFIG where Category = 'campaign' and CONVERT(DECIMAL(10,2), Value28) > 0
 AND YEAR([Start_Date]) BETWEEN YEAR(GETDATE())-1 AND YEAR(GETDATE())
 order by [Value3], [Value11], Value10, Value14 DESC
").Where(x => x.Value3 == campaign).ToList();

                var selectableAvailabilityType = context.ProductFeatureAvailabilityTypes
                    .Single(x => x.Code == ProductFeatureAvailabilityType.Selectable);

                foreach (var packagesConfig in vehicleVoluntaryPackagesConfig)
                {
                    var campaign = context.Campaigns
                        .Include(c => c.Products)
                            .ThenInclude(p => p.ProductFeatureAvailabilities)
                        .Single(x => x.Code == packagesConfig.Value3);
                    var package = campaign
                        .Products
                        .Single(x => x.Code == packagesConfig.Value11);

                    var convertVehCode = VehicleTypeVoluntary.ConvertCode(packagesConfig.Value10);
                    var vehCode = context
                        .VehicleTypeVoluntaries.Single(x => x.Code == convertVehCode);
                    var workshop = context.VehicleWorkshopTypes.Single(x => x.LookupNames.Contains(packagesConfig.Value14));

                    var vehicleTypeVoluntary = context
                        .ProductFeatures.OfType<VehicleVoluntaryFeature>()
                        .Single(x => x.VehicleTypeVoluntary == vehCode
                        && x.VehicleWorkshopType == workshop);

                    if (!package.ProductFeatureAvailabilities
                        .Any(x => x.Id == vehicleTypeVoluntary.Id))
                    {
                        context.Add(ProductFeatureAvailability.CreateBuilder(
                            package,
                            selectableAvailabilityType, vehicleTypeVoluntary)
                            .Build());

                        context.SaveChanges();
                    }

                }
            });
        }
        protected void seedProducts_VehicleVoluntaryFeature_ProductFeatureAvailabilities_Premiums(string campaign)
        {
            applyMigration(
                $"seedProducts_VehicleVoluntaryFeature_ProductFeatureAvailabilities_Premiums_{campaign}", "1.0.0", context =>
            {
                var premiumConfigs = queryConfig<TB_M_CONFIG>(@"
select 
  DISTINCT [Value3]
  ,[Value11]
  ,[Value10]
  ,[Value14]
 ,[Value12]
 ,CONVERT(DECIMAL(10,2), value25) as net_premium
 ,[Start_Date]
 ,[End_Date]
 from TB_M_CONFIG where Category = 'campaign' and CONVERT(DECIMAL(10,2), Value28) > 0
 AND YEAR([Start_Date]) BETWEEN YEAR(GETDATE())-1 AND YEAR(GETDATE())
 order by [Value3], [Value11], Value12 DESC
")
                    .Where(x => x.Value3 == TestCampaign)
                    .ToList();

                var grpBtPackagesConfig = premiumConfigs.GroupBy(x => new
                {
                    x.Value3,
                    x.Value11,
                    x.Value10,
                    x.Value14,
                    //x.net_premium,
                    //x.Start_Date,
                    //x.End_Date,
                }).ToList();

                var yearly = context.PeriodTypes.Single(x => x.Code == PeriodType.Yearly);
                var baht = context.Units.Single(x => x.Code == Unit.Baht);
                var today = DateOnly.FromDateTime(DateTime.Now);
                foreach (var config in grpBtPackagesConfig)
                {
                    var campaign = context.Campaigns
                        // Features
                        .Include(c => c.Products)
                            .ThenInclude(p => p.ProductFeatureAvailabilities)
                                .ThenInclude(pf => pf.ProductFeature)
                        .Include(c => c.Products)
                            .ThenInclude(p => p.ProductFeatureAvailabilities)
                                .ThenInclude(pf => pf.ProductFeatureAvailabilityType)
                        // Coverages
                        .Include(c => c.Products)
                            .ThenInclude(p => p.CoverageAvailabilities)
                                .ThenInclude(pf => pf.CoverageType)
                        .Include(c => c.Products)
                            .ThenInclude(p => p.CoverageAvailabilities)
                                .ThenInclude(pf => pf.CoverageLevel)
                        .AsSplitQuery()
                        .Single(x => x.Code == TestCampaign);

                    var package = campaign.Products
                        .Single(p => p.Code == config.Key.Value11);

                    var code = VehicleTypeVoluntary.ConvertCode(config.Key.Value10);
                    var vehCode = context.VehicleTypeVoluntaries.Single(x => x.Code == code);
                    var workshop = context.VehicleWorkshopTypes.Single(x => x.LookupNames.Contains(config.Key.Value14));

                    var feature = package.ProductFeatureAvailabilities
                        .Select(x => x.ProductFeature)
                        .OfType<VehicleVoluntaryFeature>()
                        .Single(x => x.VehicleTypeVoluntary == vehCode
                        && x.VehicleWorkshopType == workshop)
                        ;
                    //var avaiSumInsType = package.CoverageAvailabilities
                    //    .SingleOrDefault(ca => ca.CoverageType.Code == "COV-SUMINS");
                    foreach (var premiumConfig in config)
                    {
                        //CoverageLevel? haveCoverageLevel = null;
                        //CoverageType? haveSumInsType = null;
                        //if (avaiSumInsType != null)
                        //{
                        //    if (avaiSumInsType.CoverageLevel is CoverageAmount amount)
                        //    {
                        //        if (amount.Amount == premiumConfig.Value12)
                        //        {
                        //            haveSumInsType = avaiSumInsType.CoverageType;
                        //            haveCoverageLevel = avaiSumInsType.CoverageLevel;
                        //        }
                        //    }
                        //}

                        var startDate = DateOnly.FromDateTime(premiumConfig.Start_Date);
                        var endDate = DateOnly.FromDateTime(premiumConfig.End_Date);

                        var insRate = context.InsuranceRates
                            .SingleOrDefault(x => x.Product == package
                            && x.ProductFeature == feature
                            && x.CoverageType == null
                            && x.CoverageLevel == null
                            && x.Unit == baht
                            && x.PeriodType == yearly
                            && x.EffectiveDate == startDate
                            && x.ExpiryDate == endDate);
                        if (insRate == null)
                        {
                            insRate = InsuranceRate
                                .CreateBuilder(package, baht, yearly)
                                .WithProductFeature(feature)
                                .WithRateAmount(premiumConfig.net_premium)
                                .WithEffectiveDate(startDate)
                                .WithExpiryDate(endDate)
                                .Build();
                            context.Add(insRate);
                            context.SaveChanges();
                        }
                        else
                        {
                            insRate.UpdateRateAmount(premiumConfig.net_premium);
                        }
                    }
                }
            });
        }
        protected void seedProducts_VehicleCompulsoryFeature_ProductFeatureAvailabilities()
        {
            applyMigration("seedProducts_VehicleCompulsoryFeature_ProductFeatureAvailabilities", "1.0.0", context =>
            {
                //                var vehicleVoluntaryPackagesConfig = queryConfig<TB_M_CONFIG>(@"
                //  select 
                //  DISTINCT 
                //  [Value10]
                // ,[Description]
                // from TB_M_CONFIG where
                // VAlue1 = 'CTP' and Category = 'campaign' and Sub_Category = 'COMMON' and CONVERT(DECIMAL(10,2), Value28) > 0
                // order by [Value10], [Description] DESC
                //");

                var standardAvailabilityType = context.ProductFeatureAvailabilityTypes
                    .Single(x => x.Code == ProductFeatureAvailabilityType.Standard);

                var seatUnit = context.Units.Single(x => x.Code == Unit.Seat);
                var ccUnit = context.Units.Single(x => x.Code == Unit.CubicCentimeter);
                var kgUnit = context.Units.Single(x => x.Code == Unit.Kilogram);
                var tonUnit = context.Units.Single(x => x.Code == Unit.MetricTon);

                // รถส่วนบุคคล ( ไม่ใช้รับจ้างหรือให้เช่า ) รถยนต์นั่งไม่เกิน 7 ที่นั่ง
                addStandardProductFeatureAvailability(context, "1.10", "110", "1.10", 0, 7, seatUnit);
                // รถส่วนบุคคล ( ไม่ใช้รับจ้างหรือให้เช่า ) รถยนต์นั่งไม่เกิน 7 ที่นั่ง
                addStandardProductFeatureAvailability(context, "1.10E", "E11", "1.10", 0, 7, seatUnit);

                // รถส่วนบุคคล ( ไม่ใช้รับจ้างหรือให้เช่า ) รถยนต์นั่งโดยสารเกิน 7 ที่นั่ง ไม่เกิน 15 ที่นั่ง
                addStandardProductFeatureAvailability(context, "1.20A", "210", "1.20", 8, 15, seatUnit);
                // รถส่วนบุคคล ( ไม่ใช้รับจ้างหรือให้เช่า ) รถยนต์นั่งโดยสารเกิน 15 ที่นั่ง ถึง 20 ที่นั่ง
                addStandardProductFeatureAvailability(context, "1.20B", "210", "1.20", 16, 20, seatUnit);
                // รถส่วนบุคคล ( ไม่ใช้รับจ้างหรือให้เช่า ) รถยนต์นั่งโดยสารเเกิน 20 ที่นั่ง ถึง 40 ที่นั่ง
                addStandardProductFeatureAvailability(context, "1.20C", "210", "1.20", 21, 40, seatUnit);
                // รถส่วนบุคคล ( ไม่ใช้รับจ้างหรือให้เช่า ) รถยนต์นั่งโดยสารเเกิน 40 ที่นั่ง
                addStandardProductFeatureAvailability(context, "1.20D", "210", "1.20", 41, 999, seatUnit);

                //รถส่วนบุคคล(ไม่ใช้รับจ้างหรือให้เช่า) จักรยานยนต์ ไม่เกิน 75 ซี.ซี.
                addStandardProductFeatureAvailability(context, "1.30A", "610", "1.30", 0, 75, ccUnit);
                //รถส่วนบุคคล(ไม่ใช้รับจ้างหรือให้เช่า) จักรยานยนต์ เกิน   75 ซีซี ถึง  125 ซีซี
                addStandardProductFeatureAvailability(context, "1.30B", "610", "1.30", 76, 125, ccUnit);
                //รถส่วนบุคคล(ไม่ใช้รับจ้างหรือให้เช่า) จักรยานยนต์ เกิน 125 ซีซี ถึง  150 ซีซี
                addStandardProductFeatureAvailability(context, "1.30C", "610", "1.30", 126, 150, ccUnit);
                //รถส่วนบุคคล(ไม่ใช้รับจ้างหรือให้เช่า) จักรยานยนต์ เกิน 150 ซีซี
                addStandardProductFeatureAvailability(context, "1.30D", "610", "1.30", 151, 9999, ccUnit);

                //รถส่วนบุคคล(ไม่ใช้รับจ้างหรือให้เช่า) รถยนต์บรรทุกขนาดน้ำหนักรวม ไม่เกิน  3  ตัน
                addStandardProductFeatureAvailability(context, "1.40A", "320", "1.40", 0, 3, tonUnit);
                //รถส่วนบุคคล(ไม่ใช้รับจ้างหรือให้เช่า) รถยนต์บรรทุกขนาดน้ำหนักรวม เกิน 3 ตัน ถึง  6  ตัน
                addStandardProductFeatureAvailability(context, "1.40B", "320", "1.40", 4, 6, tonUnit);
                //รถส่วนบุคคล(ไม่ใช้รับจ้างหรือให้เช่า) รถยนต์บรรทุกขนาดน้ำหนักรวม เกิน 6 ตัน ถึง  12  ตัน
                addStandardProductFeatureAvailability(context, "1.40C", "320", "1.40", 7, 12, tonUnit);
                //รถส่วนบุคคล(ไม่ใช้รับจ้างหรือให้เช่า) รถยนต์บรรทุกขนาดน้ำหนักรวม เกิน 12 ตัน
                addStandardProductFeatureAvailability(context, "1.40D", "320", "1.40", 13, 999, tonUnit);

                // รถส่วนบุคคล ( ไม่ใช้รับจ้างหรือให้เช่า ) หัวรถลากจูง
                addStandardProductFeatureAvailability(context, "1.50", "420", "1.50");
                // รถส่วนบุคคล ( ไม่ใช้รับจ้างหรือให้เช่า ) รถพ่วง
                addStandardProductFeatureAvailability(context, "1.60", "520", "1.60");

                //---------------------------------------------------------
                // รถส่วนบุคคล ( ไม่ใช้รับจ้างหรือให้เช่า ) รถยนต์นั่งไม่เกิน 7 ที่นั่ง
                addStandardProductFeatureAvailability(context, "3.10", "730", "3.10", 0, 7, seatUnit);

                // รถส่วนบุคคล ( ไม่ใช้รับจ้างหรือให้เช่า ) รถยนต์นั่งโดยสารเกิน 7 ที่นั่ง ไม่เกิน 15 ที่นั่ง
                addStandardProductFeatureAvailability(context, "3.20A", "230", "3.20", 8, 15, seatUnit);
                // รถส่วนบุคคล ( ไม่ใช้รับจ้างหรือให้เช่า ) รถยนต์นั่งโดยสารเกิน 15 ที่นั่ง ถึง 20 ที่นั่ง
                addStandardProductFeatureAvailability(context, "3.20B", "230", "3.20", 16, 20, seatUnit);
                // รถส่วนบุคคล ( ไม่ใช้รับจ้างหรือให้เช่า ) รถยนต์นั่งโดยสารเเกิน 20 ที่นั่ง ถึง 40 ที่นั่ง
                addStandardProductFeatureAvailability(context, "3.20C", "230", "3.20", 21, 40, seatUnit);
                // รถส่วนบุคคล ( ไม่ใช้รับจ้างหรือให้เช่า ) รถยนต์นั่งโดยสารเเกิน 40 ที่นั่ง
                addStandardProductFeatureAvailability(context, "3.20D", "230", "3.20", 41, 999, seatUnit);

                //รถส่วนบุคคล(ไม่ใช้รับจ้างหรือให้เช่า) จักรยานยนต์ ไม่เกิน 75 ซี.ซี.
                addStandardProductFeatureAvailability(context, "3.30A", "630", "3.30", 0, 75, ccUnit);
                //รถส่วนบุคคล(ไม่ใช้รับจ้างหรือให้เช่า) จักรยานยนต์ เกิน   75 ซีซี ถึง  125 ซีซี
                addStandardProductFeatureAvailability(context, "3.30B", "630", "3.30", 76, 125, ccUnit);
                //รถส่วนบุคคล(ไม่ใช้รับจ้างหรือให้เช่า) จักรยานยนต์ เกิน 125 ซีซี ถึง  150 ซีซี
                addStandardProductFeatureAvailability(context, "3.30C", "630", "3.30", 126, 150, ccUnit);
                //รถส่วนบุคคล(ไม่ใช้รับจ้างหรือให้เช่า) จักรยานยนต์ เกิน 150 ซีซี
                addStandardProductFeatureAvailability(context, "3.30D", "630", "3.30", 151, 9999, ccUnit);

                //รถส่วนบุคคล(ไม่ใช้รับจ้างหรือให้เช่า) รถยนต์บรรทุกขนาดน้ำหนักรวม ไม่เกิน  3  ตัน
                addStandardProductFeatureAvailability(context, "3.40A", "320", "3.40", 0, 3, tonUnit);
                //รถส่วนบุคคล(ไม่ใช้รับจ้างหรือให้เช่า) รถยนต์บรรทุกขนาดน้ำหนักรวม เกิน 3 ตัน ถึง  6  ตัน
                addStandardProductFeatureAvailability(context, "3.40B", "320", "3.40", 4, 6, tonUnit);
                //รถส่วนบุคคล(ไม่ใช้รับจ้างหรือให้เช่า) รถยนต์บรรทุกขนาดน้ำหนักรวม เกิน 6 ตัน ถึง  12  ตัน
                addStandardProductFeatureAvailability(context, "3.40C", "320", "3.40", 7, 12, tonUnit);
                //รถส่วนบุคคล(ไม่ใช้รับจ้างหรือให้เช่า) รถยนต์บรรทุกขนาดน้ำหนักรวม เกิน 12 ตัน
                addStandardProductFeatureAvailability(context, "3.40D", "320", "3.40", 13, 999, tonUnit);

                // รถส่วนบุคคล ( ไม่ใช้รับจ้างหรือให้เช่า ) หัวรถลากจูง
                addStandardProductFeatureAvailability(context, "3.50", "420", "3.50");
                // รถส่วนบุคคล ( ไม่ใช้รับจ้างหรือให้เช่า ) รถพ่วง
                addStandardProductFeatureAvailability(context, "3.60", "520", "3.60");

                // รถส่วนบุคคลและรถที่ใช้รับจ้างหรือให้เช่า รถยนต์ป้ายแดง (การค้ารถยนต์)
                addStandardProductFeatureAvailability(context, "4.01", "801", "4.01");
                // รถส่วนบุคคลและรถที่ใช้รับจ้างหรือให้เช่า รถใช้งานเกษตรตามกฏหมายด้วยรถยนต์
                addStandardProductFeatureAvailability(context, "4.06", "804", "4.06");
                // รถส่วนบุคคลและรถที่ใช้รับจ้างหรือให้เช่า รถยนต์ประเภทอื่นๆ
                addStandardProductFeatureAvailability(context, "4.07", "806", "4.07");

            });
        }
        protected void seedProducts_VehicleCompulsoryFeature_ProductFeatureAvailabilities_Premiums()
        {
            applyMigration("seedProducts_VehicleCompulsoryFeature_ProductFeatureAvailabilities_Premiums", "1.0.0", context =>
            {
                var vehicleVoluntaryPackagesConfig = queryConfig<TB_M_CONFIG>(@"
 select 
  DISTINCT 
  [Value10]
 --,value34 as base_premium
 ,CONVERT(DECIMAL(10,2), value25) as net_premium
 ,[Start_Date]
 ,[End_Date]
 from TB_M_CONFIG where
 VAlue1 = 'CTP' and Category = 'campaign' and Sub_Category = 'COMMON' and CONVERT(DECIMAL(10,2), Value28) > 0
 order by [Value10], [Start_Date] DESC
                ");

                var yearly = context.PeriodTypes.Single(x => x.Code == PeriodType.Yearly);
                var baht = context.Units.Single(x => x.Code == Unit.Baht);
                foreach (var config in vehicleVoluntaryPackagesConfig)
                {
                    var package = context.Products
                        .Include(p => p.ProductFeatureAvailabilities)
                            .ThenInclude(pf => pf.ProductFeature)
                        .Single(p => p.Code == config.Value10 && p.ProductFeatureAvailabilities
                            .Any(pa => pa.ProductFeatureAvailabilityType.Code == ProductFeatureAvailabilityType.Standard));

                    // compul sigle feature
                    foreach (var pfAvai in package.ProductFeatureAvailabilities)
                    {
                        if (pfAvai.ProductFeature is VehicleCompulsoryFeature feature)
                        {
                            var insRate = InsuranceRate
                                .CreateBuilder(package, baht, yearly)
                                .WithProductFeature(feature)
                                .WithRateAmount(config.net_premium)
                                .WithEffectiveDate(DateOnly.FromDateTime(config.Start_Date))
                                .WithExpiryDate(DateOnly.FromDateTime(config.End_Date))
                                .Build();
                            context.Add(insRate);
                            context.SaveChanges();
                        }
                    }
                }
            });
        }
        protected void seedProducts_MasterProductPremiums()
        {
            applyMigration("seedProducts_MasterProductPremiums", "1.0.0", context =>
            {
                context.Add(PeriodType.CreateBuilder(PeriodType.Yearly)
                    .WithName("รายปี")
                    .Build());
                context.Add(PeriodType.CreateBuilder(PeriodType.Monthly)
                    .WithName("รายเดือน")
                    .Build());
                context.Add(PeriodType.CreateBuilder(PeriodType.Daily)
                    .WithName("รายวัน")
                    .Build());

                context.SaveChanges();
            });
        }

        protected void seedProducts_Units()
        {
            applyMigration("seedProducts_Units", "1.0.0", (context) =>
            {
                var timeCat = UnitCategory
                .CreateBuilder(Code.Create(UnitCategory.Time))
                .Build();
                context.Add(timeCat);
                var currencyCat = UnitCategory
                    .CreateBuilder(Code.Create(UnitCategory.Currency))
                    .Build();
                context.Add(currencyCat);
                var powerCat = UnitCategory
                   .CreateBuilder(Code.Create(UnitCategory.Power))
                   .Build();
                context.Add(powerCat);
                var volumeCat = UnitCategory
                   .CreateBuilder(Code.Create(UnitCategory.Volume))
                   .Build();
                context.Add(volumeCat);
                var quantityCat = UnitCategory
                   .CreateBuilder(Code.Create(UnitCategory.Quantity))
                   .Build();
                context.Add(quantityCat);
                var weightCat = UnitCategory
                   .CreateBuilder(Code.Create(UnitCategory.Weight))
                   .Build();
                context.Add(weightCat);

                var bth = Unit.CreateBuilder(Code.Create(Unit.Baht), currencyCat)
                    .WithSymbol("฿")
                    .Build();
                context.Add(bth);
                var usd = Unit.CreateBuilder(Code.Create(Unit.Usd), currencyCat)
                    .WithSymbol("$")
                    .Build();
                context.Add(usd);
                var day = Unit.CreateBuilder(Code.Create(Unit.Day), timeCat)
                    .WithSymbol("วัน")
                    .Build();
                context.Add(day);

                var cc = Unit.CreateBuilder(Code.Create(Unit.CubicCentimeter), volumeCat)
                    .WithSymbol("cc")
                    .Build();
                context.Add(cc);
                var seat = Unit.CreateBuilder(Code.Create(Unit.Seat), quantityCat)
                   .WithSymbol("seat")
                   .Build();
                context.Add(seat);
                var kilogram = Unit.CreateBuilder(Code.Create(Unit.Kilogram), weightCat)
                   .WithSymbol("kg")
                   .Build();
                context.Add(kilogram);
                var ton = Unit.CreateBuilder(Code.Create(Unit.MetricTon), weightCat)
                   .WithSymbol("ton")
                   .Build();
                context.Add(ton);
                var horsePower = Unit.CreateBuilder(Code.Create(Unit.Horsepower), weightCat)
                   .WithSymbol("hp")
                   .Build();
                context.Add(horsePower);

                context.SaveChanges();
            });
        }
        protected void seedProducts_VehicleUsages_สันดาบ()
        {
            applyMigration("seedProducts_VehicleUsages_สันดาบ", "1.0.0", context =>
            {
                var vehicelFuelTypes = new List<VehicleFuelType>
            {
                VehicleFuelType.CreateBuilder(Code.Create(VehicleFuelType.Combustion))
                .WithName("สันดาป")
                .Build(),

                VehicleFuelType.CreateBuilder(Code.Create(VehicleFuelType.Electric))
                .WithName("ไฟฟ้า")
                .WithPrefix("E")
                .Build(),
            };

                var listVehicleType = new List<VehicleType>
            {
                VehicleType.CreateBuilder(Code.Create("1")).WithName("ประเภทรถยนต์นั่ง").Build(),
                VehicleType.CreateBuilder(Code.Create("2")).WithName("ประเภทรถยนต์โดยสาร").Build(),
                VehicleType.CreateBuilder(Code.Create("3")).WithName("ประเภทรถยนต์บรรทุก").Build(),
                VehicleType.CreateBuilder(Code.Create("4")).WithName("ประเภทรถยนต์ลากจูง").Build(),
                VehicleType.CreateBuilder(Code.Create("5")).WithName("ประเภทรถพ่วง").Build(),
                VehicleType.CreateBuilder(Code.Create("6")).WithName("ประเภทรถจักรยานยนต์").Build(),
                VehicleType.CreateBuilder(Code.Create("7")).WithName("ประเภทรถยนต์นั่งรับจ้างสาธารณะ").Build(),
                VehicleType.CreateBuilder(Code.Create("8")).WithName("ประเภทรถยนต์เบ็ดเตล็ด").Build(),
            };

                var listVehicleUsage = new List<VehicleUsage>
            {
                VehicleUsage.CreateBuilder(Code.Create("10")).WithName("ชนิดรถยนต์ส่วนบุคคล").Build(),
                VehicleUsage.CreateBuilder(Code.Create("20")).WithName("ชนิดรถยนต์ใช้เพื่อการพาณิชย์").Build(),
                VehicleUsage.CreateBuilder(Code.Create("30")).WithName("ชนิดรถยนต์ใช้รับจ้างสาธารณะ").Build(),
                VehicleUsage.CreateBuilder(Code.Create("40")).WithName("ชนิดรถยนต์ใช้เพื่อการพาณิชย์พิเศษ").Build(),
                VehicleUsage.CreateBuilder(Code.Create("27")).WithName("ชนิดรถยนต์บรรทุกใช้ลากจูงรถพ่วงเพื่อการพาณิชย์").Build(),
                VehicleUsage.CreateBuilder(Code.Create("47")).WithName("ชนิดรถยนต์บรรทุกใช้ลากจูงรถพ่วงเพื่อการพาณิชย์พิเศษ").Build(),
                VehicleUsage.CreateBuilder(Code.Create("01")).WithName("รถยนต์ป้ายแดง").Build(),
                VehicleUsage.CreateBuilder(Code.Create("02")).WithName("รถพยาบาล").Build(),
                VehicleUsage.CreateBuilder(Code.Create("03")).WithName("รถดับเพลิง").Build(),
                VehicleUsage.CreateBuilder(Code.Create("04")).WithName("รถใช้ในการเกษตร").Build(),
                VehicleUsage.CreateBuilder(Code.Create("05")).WithName("รถใช้ในการก่อสร้าง").Build(),
                VehicleUsage.CreateBuilder(Code.Create("06")).WithName("รถอื่นๆ").Build(),
            };

                var cc = context.Units.Single(x => x.Code == Unit.CubicCentimeter);
                var seat = context.Units.Single(x => x.Code == Unit.Seat);
                var kg = context.Units.Single(x => x.Code == Unit.Kilogram);

                context.AddRange(vehicelFuelTypes);
                context.AddRange(listVehicleType);
                context.AddRange(listVehicleUsage);
                context.SaveChanges();
            });
        }
        protected void seedProducts_VehicleTypeVoluntary_สันดาป()
        {
            applyMigration("seedProducts_VehicleTypeVoluntary_สันดาป", "1.0.0", context =>
            {
                var vehCombustion = context.VehicleFuelTypes.Single(x => x.Code == VehicleFuelType.Combustion);

                var vehType1 = context.VehicleTypes.Single(x => x.Code.Value == "1");
                var vehType2 = context.VehicleTypes.Single(x => x.Code.Value == "2");
                var vehType3 = context.VehicleTypes.Single(x => x.Code.Value == "3");
                var vehType4 = context.VehicleTypes.Single(x => x.Code.Value == "4");
                var vehType5 = context.VehicleTypes.Single(x => x.Code.Value == "5");
                var vehType6 = context.VehicleTypes.Single(x => x.Code.Value == "6");
                var vehType7 = context.VehicleTypes.Single(x => x.Code.Value == "7");
                var vehType8 = context.VehicleTypes.Single(x => x.Code.Value == "8");

                var vehUsage10 = context.VehicleUsages.Single(x => x.Code.Value == "10");
                var vehUsage20 = context.VehicleUsages.Single(x => x.Code.Value == "20");
                var vehUsage30 = context.VehicleUsages.Single(x => x.Code.Value == "30");
                var vehUsage40 = context.VehicleUsages.Single(x => x.Code.Value == "40");
                var vehUsage27 = context.VehicleUsages.Single(x => x.Code.Value == "27");
                var vehUsage47 = context.VehicleUsages.Single(x => x.Code.Value == "47");
                var vehUsage01 = context.VehicleUsages.Single(x => x.Code.Value == "01");
                var vehUsage02 = context.VehicleUsages.Single(x => x.Code.Value == "02");
                var vehUsage03 = context.VehicleUsages.Single(x => x.Code.Value == "03");
                var vehUsage04 = context.VehicleUsages.Single(x => x.Code.Value == "04");
                var vehUsage05 = context.VehicleUsages.Single(x => x.Code.Value == "05");
                var vehUsage06 = context.VehicleUsages.Single(x => x.Code.Value == "06");


                var cc = context.Units.Single(x => x.Code == Unit.CubicCentimeter);
                var seat = context.Units.Single(x => x.Code == Unit.Seat);
                var kg = context.Units.Single(x => x.Code == Unit.Kilogram);

                var veh1Builder = VehicleTypeVoluntary.CreateBuilder(vehCombustion, vehType1);
                var veh2Builder = VehicleTypeVoluntary.CreateBuilder(vehCombustion, vehType2);
                var veh3Builder = VehicleTypeVoluntary.CreateBuilder(vehCombustion, vehType3);
                var veh4Builder = VehicleTypeVoluntary.CreateBuilder(vehCombustion, vehType4);
                var veh5Builder = VehicleTypeVoluntary.CreateBuilder(vehCombustion, vehType5);
                var veh6Builder = VehicleTypeVoluntary.CreateBuilder(vehCombustion, vehType6);
                var veh7Builder = VehicleTypeVoluntary.CreateBuilder(vehCombustion, vehType7);
                var veh8Builder = VehicleTypeVoluntary.CreateBuilder(vehCombustion, vehType8);
                var vehVoluntaries = new List<VehicleTypeVoluntary>
            {
                // รถยนต์นั่ง
                veh1Builder
                    .WithUsage(vehUsage10) // การใช้ส่วนบุคคล
                    .WithDescription("ใช้ส่วนบุคคล ไม่ใช้รับจ้างหรือให้เช่า")
                    .AddSize(0, 2000, cc)
                    .AddSize(2001, 99999, cc)
                    .AddCompulsory("1.10")
                    .Build(),
                veh1Builder
                    .WithUsage(vehUsage20) // การใช้เพื่อการพาณิชย์
                    .WithDescription("ใช้เพื่อการพาณิชย์ ไม่ใช้รับจ้างสาธารณะ")
                    .AddSize(0, 2000, cc)
                    .AddSize(2001, 99999, cc)
                    .AddCompulsory("1.10")
                    .AddCompulsory("2.10")
                    .Build(),

                // รถยนต์โดยสาร
                veh2Builder
                    .WithUsage(vehUsage10) // การใช้ส่วนบุคคล
                    .WithDescription("ใช้ส่วนบุคคล ไม่ใช้รับจ้างหรือให้เช่า")
                    .AddSize(0, 20, seat)
                    .AddSize(21, 40, seat)
                    .AddSize(41, 999, seat)
                    .AddCompulsory("1.20")
                    .Build(),
                veh2Builder
                    .WithUsage(vehUsage20) // การใช้เพื่อการพาณิชย์
                    .WithDescription("ใช้เพื่อการพาณิชย์ ไม่ใช้รับจ้างสาธารณะ")
                    .AddSize(0, 20, seat)
                    .AddSize(21, 40, seat)
                    .AddSize(41, 999, seat)
                    .AddCompulsory("1.20")
                    .AddCompulsory("2.20")
                    .Build(),
                veh2Builder
                    .WithUsage( vehUsage30) // การใช้รับจ้างสาธารณะ
                    .WithDescription("ใช้รับจ้างสาธารณะ")
                    .AddSize(0, 20, seat)
                    .AddSize(21, 40, seat)
                    .AddSize(41, 999, seat)
                    .AddCompulsory("3.20")
                    .AddCompulsory("4.07", "สำหรับรถสี่ล้อเล็กหรือรถโดยสารขนาดเล็กที่มีขนาดที่นั่งไม่เกิน 15 ที่นั่งรหัสทะเบียน 10-19 20-29\r\nและ 30-39")
                    .Build(),

                // รถยนต์บรรทุก
                veh3Builder
                    .WithUsage(vehUsage20) // การใช้เพื่อการพาณิชย์
                    .WithDescription("ใช้เพื่อการพาณิชย์ ไม่ใช้เพื่อการบรรทุก และขนส่ง\r\nสินค้าที่มีความเสี่ยงภัยสูง เช่น เชื้อเพลิง กรด แก๊ส\r\nและไม่ใช้ลากจูงรถพ่วง")
                    .AddSize(0, 4000, kg)
                    .AddSize(4001, 12000, kg)
                    .AddSize(12001, 999999, kg)
                    .AddCompulsory("1.40")
                    .AddCompulsory("2.40")
                    .AddCompulsory("3.40")
                    .Build(),
                 veh3Builder
                    .WithUsage(vehUsage40) // การใช้เพื่อการพาณิชย์พิเศษ
                    .WithDescription("ใช้เพื่อการพาณิชย์พิเศษ การบรรทุก และขนส่งสินค้า\r\nที่มีความเสี่ยงภัยสูง เช่น เชื้อเพลิง กรด แก๊ส และไม่\r\nใช้ลากจูงรถพ่วง")
                    .AddSize(0, 4000, kg)
                    .AddSize(4001, 12000, kg)
                    .AddSize(12001, 999999, kg)
                    .AddCompulsory("1.42")
                    .AddCompulsory("2.42")
                    .AddCompulsory("3.42")
                    .Build(),
                 veh3Builder
                    .WithUsage(vehUsage27)
                    .WithDescription("ใช้เพื่อการพาณิชย์ ไม่ใช้เพื่อการบรรทุก และขนส่ง\r\nสินค้าที่มีความเสี่ยงภัยสูง เช่น เชื้อเพลิง กรด แก๊ส")
                    .AddSize(0, 4000, kg)
                    .AddSize(4001, 12000, kg)
                    .AddSize(12001, 999999, kg)
                    .AddCompulsory("1.40")
                    .AddCompulsory("2.40")
                    .AddCompulsory("3.40")
                    .Build(),
                 veh3Builder
                    .WithUsage(vehUsage47)
                    .WithDescription("ใช้เพื่อการพาณิชย์พิเศษ การบรรทุก และขนส่งสินค้า\r\nที่มีความเสี่ยงภัยสูง เช่น เชื้อเพลิง กรด แก๊ส")
                    .AddSize(0, 4000, kg)
                    .AddSize(4001, 12000, kg)
                    .AddSize(12001, 999999, kg)
                    .AddCompulsory("1.42")
                    .AddCompulsory("2.42")
                    .AddCompulsory("3.42")
                    .Build(),

                 // รถยนต์ลากจูง
                 veh4Builder
                    .WithUsage(vehUsage20)
                    .WithDescription("ใช้เพื่อการพาณิชย์ ไม่ใช้เพื่อการบรรทุก และขนส่ง\r\nสินค้าที่มีความเสี่ยงภัยสูง เช่น เชื้อเพลิง กรด แก๊ส")
                    .AddSize(0, 8000, kg)
                    .AddSize(8001, 99999, kg)
                    .AddCompulsory("1.50")
                    .AddCompulsory("2.50")
                    .AddCompulsory("3.50")
                    .Build(),

                 // รถพ่วง
                 veh5Builder
                    .WithUsage(vehUsage20)
                    .WithDescription("ใช้เพื่อการพาณิชย์ ไม่ใช้เพื่อการบรรทุก และขนส่ง\r\nสินค้าที่มีความเสี่ยงภัยสูง เช่น เชื้อเพลิง กรด แก๊ส")
                    .AddSize(0, 30000, kg)
                    .AddSize(30001, 999999, kg)
                    .AddCompulsory("1.60")
                    .AddCompulsory("2.60")
                    .AddCompulsory("3.60")
                    .Build(),
                  veh5Builder
                    .WithUsage(vehUsage40)
                    .WithDescription("ใช้เพื่อการพาณิชย์พิเศษ การบรรทุก และขนส่งสินค้า\r\nที่มีความเสี่ยงภัยสูง เช่น เชื้อเพลิง กรด แก๊ส")
                    .AddSize(0, 30000, kg)
                    .AddSize(30001, 999999, kg)
                    .AddCompulsory("1.60")
                    .AddCompulsory("2.60")
                    .AddCompulsory("3.60")
                    .Build(),

                  // รถจักรยานยนต์
                  veh6Builder
                    .WithUsage(vehUsage10)
                    .WithDescription("ใช้ส่วนบุคคล ไม่ใช้รับจ้างหรือให้เช่า")
                    .AddSize(0, 125, cc)
                    .AddSize(126, 250, cc)
                    .AddSize(251, 9999, cc)
                    .AddCompulsory("1.30")
                    .Build(),
                  veh6Builder
                    .WithUsage(vehUsage20)
                    .WithDescription("ใช้เพื่อการพาณิชย์ ไม่ใช้รับจ้างสาธารณะ")
                    .AddSize(0, 125, cc)
                    .AddSize(126, 250, cc)
                    .AddSize(251, 9999, cc)
                    .AddCompulsory("1.30")
                    .AddCompulsory("2.30")
                    .AddCompulsory("3.30")
                    .Build(),
                  veh6Builder
                    .WithUsage(vehUsage30)
                    .WithDescription("ใช้รับจ้างสาธารณะ")
                    .AddSize(0, 125, cc)
                    .AddSize(126, 250, cc)
                    .AddSize(251, 9999, cc)
                    .AddCompulsory("1.30")
                    .AddCompulsory("2.30")
                    .AddCompulsory("3.30")
                    .Build(),

                  // รถยนต์นั่งรับจ้าง สาธารณะ
                  veh7Builder
                    .WithUsage(vehUsage30)
                    .WithDescription("ใช้รับจ้างสาธารณะ")
                    .AddSize(0, 1000, cc)
                    .AddSize(1001, 2000, cc)
                    .AddSize(2001, 99999, cc)
                    .AddCompulsory("3.10")
                    .Build(),

                  // รถยนต์เบ็ดเตล็ด
                  veh8Builder
                    .WithUsage(vehUsage01)
                    .WithDescription("ใช้เพื่อการค้ารถยนต์ และการซ่อมรถยนต์")
                    .AddCompulsory("4.01")
                    .Build(),
                  veh8Builder
                    .WithUsage(vehUsage02)
                    .WithDescription("รถพยาบาล")
                    .AddSize(0, 2000, cc)
                    .AddSize(2001, 99999, cc)
                    .AddCompulsory("4.07", "ให้ใช้ตามลักษณะการจดทะเบียนของรถ กรณีไม่สามารถ\r\nระบุประเภทได้ ให้ใช้รหัส 4.07")
                    .Build(),
                  veh8Builder
                    .WithUsage(vehUsage03)
                    .WithDescription("รถดับเพลิง")
                    .AddSize(0, 12000, kg)
                    .AddSize(12001, 999999, kg)
                    .AddCompulsory("4.07", "ให้ใช้ตามลักษณะการจดทะเบียนของรถ กรณีไม่สามารถ\r\nระบุประเภทได้ ให้ใช้รหัส 4.07")
                    .Build(),
                  veh8Builder
                    .WithUsage(vehUsage04)
                    .WithDescription("รถใช้ในการเกษตร")
                    .AddSize(0, 12000, kg)
                    .AddSize(12001, 999999, kg)
                    .AddCompulsory("4.06", "ให้ใช้ตามลักษณะการจดทะเบียนของรถ กรณีไม่สามารถ\r\nระบุประเภทได้ ให้ใช้รหัส 4.07")
                    .Build(),
                  veh8Builder
                    .WithUsage(vehUsage05)
                    .WithDescription("รถใช้ในการก่อสร้าง")
                    .AddSize(0, 12000, kg)
                    .AddSize(12001, 999999, kg)
                    .AddCompulsory("4.07", "ให้ใช้ตามลักษณะการจดทะเบียนของรถ กรณีไม่สามารถ\r\nระบุประเภทได้ ให้ใช้รหัส 4.07")
                    .Build(),
                  veh8Builder
                    .WithUsage(vehUsage06)
                    .WithDescription("รถอื่นๆ")
                    .AddCompulsory("4.07")
                    .Build(),
            };

                context.AddRange(vehVoluntaries);

                context.SaveChanges();
            });
        }
        protected void seedProducts_VehicleUsages_ไฟฟ้า()
        {
            applyMigration("seedProducts_VehicleUsages_ไฟฟ้า", "1.0.0", context =>
            {
                var listVehicleUsage = new List<VehicleUsage>
                {
                    VehicleUsage.CreateBuilder(Code.Create("1")).WithName("ชนิดรถยนต์ส่วนบุคคล").Build(),
                    VehicleUsage.CreateBuilder(Code.Create("2")).WithName("ชนิดรถยนต์ใช้เพื่อการพาณิชย์").Build(),
                    VehicleUsage.CreateBuilder(Code.Create("3")).WithName("ชนิดรถยนต์ใช้รับจ้างสาธารณะ").Build(),
                    VehicleUsage.CreateBuilder(Code.Create("4")).WithName("ชนิดรถยนต์ใช้เพื่อการพาณิชย์พิเศษ").Build(),
                    VehicleUsage.CreateBuilder(Code.Create("5")).WithName("ชนิดรถยนต์บรรทุกใช้ลากจูงรถพ่วงเพื่อการพาณิชย์").Build(),
                    VehicleUsage.CreateBuilder(Code.Create("6")).WithName("ชนิดรถยนต์บรรทุกใช้ลากจูงรถพ่วงเพื่อการพาณิชย์พิเศษ").Build(),
                    VehicleUsage.CreateBuilder(Code.Create("A")).WithName("รถยนต์ป้ายแดง").Build(),
                    VehicleUsage.CreateBuilder(Code.Create("B")).WithName("รถพยาบาล").Build(),
                    VehicleUsage.CreateBuilder(Code.Create("C")).WithName("รถดับเพลิง").Build(),
                    VehicleUsage.CreateBuilder(Code.Create("D")).WithName("รถใช้ในการเกษตร").Build(),
                    VehicleUsage.CreateBuilder(Code.Create("E")).WithName("รถใช้ในการก่อสร้าง").Build(),
                    VehicleUsage.CreateBuilder(Code.Create("F")).WithName("รถอื่นๆ").Build(),
                };

                context.AddRange(listVehicleUsage);
                context.SaveChanges();
            });
        }
        protected void seedProducts_VehicleTypeVoluntary_ไฟฟ้า()
        {
            applyMigration("seedProducts_VehicleTypeVoluntary_ไฟฟ้า", "1.0.0", context =>
            {
                var vehElectric = context.VehicleFuelTypes.Single(x => x.Code == VehicleFuelType.Electric);

                var vehType1 = context.VehicleTypes.Single(x => x.Code.Value == "1");
                var vehType2 = context.VehicleTypes.Single(x => x.Code.Value == "2");
                var vehType3 = context.VehicleTypes.Single(x => x.Code.Value == "3");
                var vehType4 = context.VehicleTypes.Single(x => x.Code.Value == "4");
                var vehType5 = context.VehicleTypes.Single(x => x.Code.Value == "5");
                var vehType6 = context.VehicleTypes.Single(x => x.Code.Value == "6");
                var vehType7 = context.VehicleTypes.Single(x => x.Code.Value == "7");
                var vehType8 = context.VehicleTypes.Single(x => x.Code.Value == "8");

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

                var hp = context.Units.Single(x => x.Code == Unit.Horsepower);
                var seat = context.Units.Single(x => x.Code == Unit.Seat);
                var kg = context.Units.Single(x => x.Code == Unit.Kilogram);

                // รถยนต์นั่ง
                var veh1Voluntaries = VehicleTypeVoluntary
                    .CreateBuilder(vehElectric, vehType1)
                    .WithUsage(vehUsage1) // การใช้ส่วนบุคคล
                        .WithDescription("ใช้ส่วนบุคคล ไม่ใช้รับจ้างหรือให้เช่า")
                        .AddSize(0, 175, hp)
                        .AddSize(176, 9999, hp)
                        .AddCompulsories("1.10")
                        .Next()
                    .WithUsage(vehUsage2) // การใช้เพื่อการพาณิชย์
                        .WithDescription("ใช้เพื่อการพาณิชย์ ไม่ใช้รับจ้างสาธารณะ")
                        .AddSize(0, 175, hp)
                        .AddSize(176, 9999, hp)
                        .AddCompulsories("1.10", "2.10")
                        .Next()
                    .End();
                context.AddRange(veh1Voluntaries);

                // รถยนต์โดยสาร
                var veh2Voluntaries = VehicleTypeVoluntary
                    .CreateBuilder(vehElectric, vehType2)
                    .WithUsage(vehUsage1) // การใช้ส่วนบุคคล
                        .WithDescription("ใช้ส่วนบุคคล ไม่ใช้รับจ้างหรือให้เช่า")
                        .AddSize(0, 20, seat)
                        .AddSize(21, 40, seat)
                        .AddSize(41, 999, seat)
                        .AddCompulsories("1.20")
                        .Next()
                    .WithUsage(vehUsage2) // การใช้เพื่อการพาณิชย์
                        .WithDescription("ใช้เพื่อการพาณิชย์ ไม่ใช้รับจ้างสาธารณะ")
                        .AddSize(0, 20, seat)
                        .AddSize(21, 40, seat)
                        .AddSize(41, 999, seat)
                        .AddCompulsories("1.20", "2.20")
                        .Next()
                    .WithUsage(vehUsage3) // การใช้รับจ้างสาธารณะ
                        .WithDescription("ใช้รับจ้างสาธารณะ")
                        .AddSize(0, 20, seat)
                        .AddSize(21, 40, seat)
                        .AddSize(41, 999, seat)
                        .AddCompulsories("3.20", "4.07")
                        .Next()
                    .End();
                context.AddRange(veh2Voluntaries);

                // รถยนต์บรรทุก
                var veh3Voluntaries = VehicleTypeVoluntary
                    .CreateBuilder(vehElectric, vehType3)
                    .WithUsage(vehUsage2) // การใช้เพื่อการพาณิชย์
                        .WithDescription("ใช้เพื่อการพาณิชย์ ไม่ใช้เพื่อการบรรทุก และขนส่ง\r\nสินค้าที่มีความเสี่ยงภัยสูง เช่น เชื้อเพลิง กรด แก๊ส\r\nและไม่ใช้ลากจูงรถพ่วง")
                        .AddSize(0, 4000, kg)
                        .AddSize(4001, 12000, kg)
                        .AddSize(12001, 999999, kg)
                        .AddCompulsories("1.40", "2.40", "3.40")
                        .Next()
                    .WithUsage(vehUsage4) // การใช้เพื่อการพาณิชย์พิเศษ
                        .WithDescription("ใช้เพื่อการพาณิชย์พิเศษ การบรรทุก และขนส่งสินค้า\r\nที่มีความเสี่ยงภัยสูง เช่น เชื้อเพลิง กรด แก๊ส และไม่\r\nใช้ลากจูงรถพ่วง")
                        .AddSize(0, 4000, kg)
                        .AddSize(4001, 12000, kg)
                        .AddSize(12001, 999999, kg)
                        .AddCompulsories("1.40", "2.40", "3.40")
                        .Next()
                    .WithUsage(vehUsage5) // การใช้ลากจูงรถพ่วงเพื่อการพาณิชย์
                        .WithDescription("ใช้เพื่อการพาณิชย์ ไม่ใช้เพื่อการบรรทุก และขนส่ง\r\nสินค้าที่มีความเสี่ยงภัยสูง เช่น เชื้อเพลิง กรด แก๊ส")
                        .AddSize(0, 4000, kg)
                        .AddSize(4001, 12000, kg)
                        .AddSize(12001, 999999, kg)
                        .AddCompulsories("1.42", "2.42", "3.42")
                        .Next()
                    .WithUsage(vehUsage6) // การใช้ลากจูงรถพ่วงเพื่อการพาณิชย์พิเศษ
                        .WithDescription("ใช้เพื่อการพาณิชย์พิเศษ การบรรทุก และขนส่งสินค้า\r\nที่มีความเสี่ยงภัยสูง เช่น เชื้อเพลิง กรด แก๊ส")
                        .AddSize(0, 4000, kg)
                        .AddSize(4001, 12000, kg)
                        .AddSize(12001, 999999, kg)
                        .AddCompulsories("1.42", "2.42", "3.42")
                        .Next()
                    .End();
                context.AddRange(veh3Voluntaries);

                // รถยนต์ลากจูง
                var veh4Voluntaries = VehicleTypeVoluntary
                    .CreateBuilder(vehElectric, vehType4)
                    .WithUsage(vehUsage2) // การใช้เพื่อการพาณิชย์
                        .WithDescription("ใช้เพื่อการพาณิชย์ ไม่ใช้เพื่อการบรรทุก และขนส่ง\r\nสินค้าที่มีความเสี่ยงภัยสูง เช่น เชื้อเพลิง กรด แก๊ส")
                        .AddSize(0, 8000, kg)
                        .AddSize(8001, 99999, kg)
                        .AddCompulsories("1.50", "2.50", "3.50")
                        .Next()
                    .End();
                context.AddRange(veh4Voluntaries);

                // รถพ่วง
                var veh5Voluntaries = VehicleTypeVoluntary
                   .CreateBuilder(vehElectric, vehType5)
                   .WithUsage(vehUsage2) // การใช้เพื่อการพาณิชย์
                       .WithDescription("ใช้เพื่อการพาณิชย์ ไม่ใช้เพื่อการบรรทุก และขนส่ง\r\nสินค้าที่มีความเสี่ยงภัยสูง เช่น เชื้อเพลิง กรด แก๊ส")
                       .AddSize(0, 30000, kg)
                       .AddSize(30001, 999999, kg)
                       .AddCompulsories("1.60", "2.60", "3.60")
                       .Next()
                    .WithUsage(vehUsage4) // การใช้เพื่อการพาณิชย์พิเศษ
                       .WithDescription("ใช้เพื่อการพาณิชย์พิเศษ การบรรทุก และขนส่งสินค้า\r\nที่มีความเสี่ยงภัยสูง เช่น เชื้อเพลิง กรด แก๊ส")
                       .AddSize(0, 30000, kg)
                       .AddSize(30001, 999999, kg)
                       .AddCompulsories("1.60", "2.60", "3.60")
                       .Next()
                   .End();
                context.AddRange(veh5Voluntaries);

                // รถจักรยานยนต์
                var veh6Voluntaries = VehicleTypeVoluntary
                   .CreateBuilder(vehElectric, vehType6)
                   .WithUsage(vehUsage1) // การใช้ส่วนบุคคล
                       .WithDescription("ใช้ส่วนบุคคล ไม่ใช้รับจ้างหรือให้เช่า")
                       .AddSize(0, 8.02m, hp)
                       .AddSize(8.03m, 16.03m, hp)
                       .AddSize(16.04m, 999m, hp)
                       .AddCompulsories("1.30")
                       .Next()
                    .WithUsage(vehUsage2) // การใช้เพื่อการพาณิชย์
                       .WithDescription("ใช้เพื่อการพาณิชย์ ไม่ใช้รับจ้างสาธารณะ")
                       .AddSize(0, 8.02m, hp)
                       .AddSize(8.03m, 16.03m, hp)
                       .AddSize(16.04m, 999m, hp)
                       .AddCompulsories("1.30", "2.30", "3.30")
                       .Next()
                    .WithUsage(vehUsage3) // การใช้รับจ้างสาธารณะ
                       .WithDescription("ใช้รับจ้างสาธารณะ")
                       .AddSize(0, 8.02m, hp)
                       .AddSize(8.03m, 16.03m, hp)
                       .AddSize(16.04m, 999m, hp)
                       .AddCompulsories("1.30", "2.30", "3.30")
                       .Next()
                   .End();
                context.AddRange(veh6Voluntaries);

                // รถยนต์นั่งรับจ้างสาธารณะ
                var veh7Voluntaries = VehicleTypeVoluntary
                    .CreateBuilder(vehElectric, vehType7)
                    .WithUsage(vehUsage3) // การใช้รับจ้างสาธารณะ
                        .WithDescription("ใช้รับจ้างสาธารณะ")
                        .AddSize(0, 64.11m, hp)
                        .AddSize(64.12m, 175, hp)
                        .AddSize(176, 9999, hp)
                        .AddCompulsories("3.10")
                        .Next()
                    .End();
                context.AddRange(veh7Voluntaries);

                // รถยนต์เบ็ดเตล็ด
                var veh8Voluntaries = VehicleTypeVoluntary
                    .CreateBuilder(vehElectric, vehType8)
                    .WithUsage(vehUsageA) // รถยนต์ป้ายแดง
                        .WithDescription("ใช้เพื่อการค้ารถยนต์ และการซ่อมรถยนต์")
                        .AddCompulsories("4.01")
                        .Next()
                    .WithUsage(vehUsageB) // รถพยาบาล
                        .WithDescription("รถพยาบาล")
                        .AddSize(0, 175, hp)
                        .AddSize(176, 9999, hp)
                        .AddCompulsories("4.07")
                        .Next()
                    .WithUsage(vehUsageC) // รถดับเพลิง
                        .WithDescription("รถดับเพลิง")
                        .AddSize(0, 12000, kg)
                        .AddSize(12001, 999999, kg)
                        .AddCompulsories("4.07")
                        .Next()
                    .WithUsage(vehUsageD) // รถใช้ในการเกษตร
                        .WithDescription("รถใช้ในการเกษตร")
                        .AddSize(0, 12000, kg)
                        .AddSize(12001, 999999, kg)
                        .AddCompulsories("4.06")
                        .Next()
                    .WithUsage(vehUsageE) // รถใช้ในการก่อสร้าง
                        .WithDescription("รถใช้ในการก่อสร้าง")
                        .AddSize(0, 12000, kg)
                        .AddSize(12001, 999999, kg)
                        .AddCompulsories("4.07")
                        .Next()
                    .WithUsage(vehUsageF) // รถอื่น ๆ
                        .WithDescription("รถอื่น ๆ")
                        .AddCompulsories("4.07")
                        .Next()
                    .End();
                context.AddRange(veh8Voluntaries);

                context.SaveChanges();
            });
        }
        protected void seedProducts_VehicleWorkshopTypes()
        {
            applyMigration("seedProducts_VehicleWorkshopTypes", "1.0.0", context =>
            {
                var garage = VehicleWorkshopType
                    .CreateBuilder(Code.Create(VehicleWorkshopType.Garage), Name.Create("ซ่อมอู่"))
                    .WithLookupNames("ซ่อมอู่", "อู่")
                    .Build();
                context.Add(garage);

                var dealer = VehicleWorkshopType
                    .CreateBuilder(Code.Create(VehicleWorkshopType.Dealer), Name.Create("ซ่อมห้าง"))
                    .WithLookupNames("ซ่อมห้าง", "ห้าง", "ซ่อมห้างญี่ปุ่น", "ซ่อมห้างยุโรป")
                    .Build();
                context.Add(dealer);

                context.SaveChanges();
            });
        }

        protected void seedProducts_ProductFeatureTypes()
        {
            applyMigration("seedProducts_ProductFeatureTypes", "1.0.0", context =>
            {
                context.Add(ProductFeatureType
                    .CreateBuilder(Code.Create(ProductFeatureType.VehicleVoluntary))
                    .Build());

                context.Add(ProductFeatureType
                    .CreateBuilder(Code.Create(ProductFeatureType.VehicleCompulsory))
                    .Build());

                context.Add(ProductFeatureType
                   .CreateBuilder(Code.Create(ProductFeatureType.VehicleBrand))
                   .Build());

                context.Add(ProductFeatureType
                   .CreateBuilder(Code.Create(ProductFeatureType.VehicleModel))
                   .Build());

                context.Add(ProductFeatureType
                   .CreateBuilder(Code.Create(ProductFeatureType.VehiclePriceGroup))
                   .Build());

                context.SaveChanges();
            });
        }
        protected void seedProducts_VehicleVoluntaryFeatures()
        {
            applyMigration("seedProducts_VehicleVoluntaryFeatures", "1.0.0", context =>
            {
                var informFeaturesConfig = queryConfig<TB_M_CONFIG>(@"
 SELECT DISTINCT Value14, Value18
 from TB_M_CONFIG where Category = 'campaign' 
 and CONVERT(DECIMAL(10,2), Value28) > 0 
 AND YEAR([Start_Date]) BETWEEN YEAR(GETDATE())-1 AND YEAR(GETDATE())
 AND [Start_Date] < End_Date
");

                var informMotoType = context.ProductFeatureTypes.Single(x => x.Code == ProductFeatureType.VehicleVoluntary);
                // convert vehivle volun code
                foreach (var informFeature in informFeaturesConfig)
                {
                    var vehicleTypeVoluntaryCode = VehicleTypeVoluntary.ConvertCode(informFeature.Value18);
                    var vehicleTypeVoluntary = context
                        .VehicleTypeVoluntaries
                        .Single(x => x.Code == vehicleTypeVoluntaryCode);
                    var workshop = context.VehicleWorkshopTypes
                        .Single(x => x.LookupNames.Contains(informFeature.Value14));
                    var informMotorFeature = context.ProductFeatures.OfType<VehicleVoluntaryFeature>()
                        .SingleOrDefault(x => x.VehicleWorkshopType == workshop
                        && x.VehicleTypeVoluntary == vehicleTypeVoluntary);
                    if (informMotorFeature != null) continue;

                    informMotorFeature = new VehicleVoluntaryFeature(informMotoType)
                    {
                        VehicleWorkshopType = workshop,
                        VehicleTypeVoluntary = vehicleTypeVoluntary
                    };

                    context.Add(informMotorFeature);

                    context.SaveChanges();
                }
            });
        }

        protected void seedProducts_VehicleCompulsoryFeatures()
        {
            applyMigration("seedProducts_VehicleCompulsoryFeatures", "1.0.0", ctx =>
            {
                //                var informFeaturesConfig = queryConfig<TB_M_CONFIG>(@"
                //           select
                //            DISTINCT
                //            [Value10]
                //           ,[Description]
                //--,value34 as base_premium
                //,CONVERT(DECIMAL(10, 2), value25) as net_premium
                //,[Start_Date]
                //,[End_Date]
                //           from TB_M_CONFIG where
                //           VAlue1 = 'CTP' and Category = 'campaign' and Sub_Category = 'COMMON' and CONVERT(DECIMAL(10,2), Value28) > 0
                //order by[Value10], [Start_Date] DESC

                //");

                var seatUnit = ctx.Units.Single(x => x.Code == Unit.Seat);
                var ccUnit = ctx.Units.Single(x => x.Code == Unit.CubicCentimeter);
                var kgUnit = ctx.Units.Single(x => x.Code == Unit.Kilogram);
                var tonUnit = ctx.Units.Single(x => x.Code == Unit.MetricTon);

                var informMotoType = ctx.ProductFeatureTypes.Single(x => x.Code == ProductFeatureType.VehicleCompulsory);

                // separate size
                // รถส่วนบุคคล ( ไม่ใช้รับจ้างหรือให้เช่า ) รถยนต์นั่ง
                var vehVol1Codes = new List<string> { "210", "E21", "E11", "110" };
                foreach (var vehVolCode in vehVol1Codes)
                {
                    var veh = ctx.VehicleTypeVoluntaries
                        .Include(vv => vv.VehicleTypeCompulsories)
                        .Single(x => x.Code == vehVolCode);

                    // Seat
                    var com11 = veh.VehicleTypeCompulsories.SingleOrDefault(x => x.Code == "1.10");
                    if (com11 != null)
                    {
                        // Seat
                        ctx.Add(new VehicleCompulsoryFeature(informMotoType, veh, com11, 0, 7, seatUnit));
                        //ctx.Add(new VehicleCompulsoryFeature(informMotoType, veh, com11, 8, 15, seatUnit));
                        //ctx.Add(new VehicleCompulsoryFeature(informMotoType, veh, com11, 16, 20, seatUnit));
                        //ctx.Add(new VehicleCompulsoryFeature(informMotoType, veh, com11, 21, 40, seatUnit));
                        //ctx.Add(new VehicleCompulsoryFeature(informMotoType, veh, com11, 41, 999, seatUnit));

                        ctx.SaveChanges();
                    }

                    var com12 = veh.VehicleTypeCompulsories.SingleOrDefault(x => x.Code == "1.20");
                    if (com12 != null)
                    {
                        // Seat
                        ctx.Add(new VehicleCompulsoryFeature(informMotoType, veh, com12, 0, 7, seatUnit));
                        ctx.Add(new VehicleCompulsoryFeature(informMotoType, veh, com12, 8, 15, seatUnit));
                        ctx.Add(new VehicleCompulsoryFeature(informMotoType, veh, com12, 16, 20, seatUnit));
                        ctx.Add(new VehicleCompulsoryFeature(informMotoType, veh, com12, 21, 40, seatUnit));
                        ctx.Add(new VehicleCompulsoryFeature(informMotoType, veh, com12, 41, 999, seatUnit));

                        ctx.SaveChanges();
                    }
                }
                // รถส่วนบุคคล ( ไม่ใช้รับจ้างหรือให้เช่า ) จักรยานยนต์ ซี.ซี.
                var vehVol2Codes = new List<string> { "610", "E61" };
                foreach (var vehVolCode in vehVol2Codes)
                {
                    var veh = ctx.VehicleTypeVoluntaries
                        .Include(vv => vv.VehicleTypeCompulsories)
                        .Single(x => x.Code == vehVolCode);

                    // CC
                    var com13 = veh.VehicleTypeCompulsories.SingleOrDefault(x => x.Code == "1.30");
                    if (com13 != null)
                    {
                        ctx.Add(new VehicleCompulsoryFeature(informMotoType, veh, com13, 0, 75, ccUnit));
                        ctx.Add(new VehicleCompulsoryFeature(informMotoType, veh, com13, 76, 125, ccUnit));
                        ctx.Add(new VehicleCompulsoryFeature(informMotoType, veh, com13, 126, 150, ccUnit));
                        ctx.Add(new VehicleCompulsoryFeature(informMotoType, veh, com13, 151, 9999, ccUnit));

                        ctx.SaveChanges();
                    }
                }
                // รถส่วนบุคคล ( ไม่ใช้รับจ้างหรือให้เช่า ) รถยนต์บรรทุกขนาดน้ำหนักรวม ตัน
                var vehVol3Codes = new List<string> { "320", "327" };
                foreach (var vehVolCode in vehVol3Codes)
                {
                    var veh = ctx.VehicleTypeVoluntaries
                        .Include(vv => vv.VehicleTypeCompulsories)
                        .Single(x => x.Code == vehVolCode);

                    // TON
                    var com14 = veh.VehicleTypeCompulsories.SingleOrDefault(x => x.Code == "1.40");
                    if (com14 != null)
                    {
                        ctx.Add(new VehicleCompulsoryFeature(informMotoType, veh, com14, 0, 3, tonUnit));
                        ctx.Add(new VehicleCompulsoryFeature(informMotoType, veh, com14, 4, 6, tonUnit));
                        ctx.Add(new VehicleCompulsoryFeature(informMotoType, veh, com14, 7, 12, tonUnit));
                        ctx.Add(new VehicleCompulsoryFeature(informMotoType, veh, com14, 13, 999, tonUnit));

                        ctx.SaveChanges();
                    }
                }
                var vehVol4Codes = new List<string> { "420", "520", "540" };
                foreach (var vehVolCode in vehVol4Codes)
                {
                    var veh = ctx.VehicleTypeVoluntaries
                        .Include(vv => vv.VehicleTypeCompulsories)
                        .Single(x => x.Code == vehVolCode);

                    var com15 = veh.VehicleTypeCompulsories.SingleOrDefault(x => x.Code == "1.50");
                    if (com15 != null)
                    {
                        ctx.Add(new VehicleCompulsoryFeature(informMotoType, veh, com15));
                        ctx.SaveChanges();
                    }
                    var com16 = veh.VehicleTypeCompulsories.SingleOrDefault(x => x.Code == "1.60");
                    if (com16 != null)
                    {
                        ctx.Add(new VehicleCompulsoryFeature(informMotoType, veh, com16));
                        ctx.SaveChanges();
                    }
                }

                // รถที่ใช้รับจ้างหรือให้เช่า รถยนต์นั่งไม่เกิน ที่นั่ง
                var vehPubVol1Codes = new List<string> { "730", "E23", "E73", "230" };
                foreach (var vehVolCode in vehPubVol1Codes)
                {
                    var veh = ctx.VehicleTypeVoluntaries
                        .Include(vv => vv.VehicleTypeCompulsories)
                        .Single(x => x.Code == vehVolCode);

                    // Seat
                    var com31 = veh.VehicleTypeCompulsories.SingleOrDefault(x => x.Code == "3.10");
                    if (com31 != null)
                    {
                        ctx.Add(new VehicleCompulsoryFeature(informMotoType, veh, com31, 0, 7, seatUnit));
                        //ctx.Add(new VehicleCompulsoryFeature(informMotoType, veh, com31, 8, 15, seatUnit));
                        //ctx.Add(new VehicleCompulsoryFeature(informMotoType, veh, com31, 16, 20, seatUnit));
                        //ctx.Add(new VehicleCompulsoryFeature(informMotoType, veh, com31, 21, 40, seatUnit));
                        //ctx.Add(new VehicleCompulsoryFeature(informMotoType, veh, com31, 41, 999, seatUnit));

                        ctx.SaveChanges();
                    }

                    var com32 = veh.VehicleTypeCompulsories.SingleOrDefault(x => x.Code == "3.20");
                    if (com32 != null)
                    {
                        ctx.Add(new VehicleCompulsoryFeature(informMotoType, veh, com32, 0, 7, seatUnit));
                        ctx.Add(new VehicleCompulsoryFeature(informMotoType, veh, com32, 8, 15, seatUnit));
                        ctx.Add(new VehicleCompulsoryFeature(informMotoType, veh, com32, 16, 20, seatUnit));
                        ctx.Add(new VehicleCompulsoryFeature(informMotoType, veh, com32, 21, 40, seatUnit));
                        ctx.Add(new VehicleCompulsoryFeature(informMotoType, veh, com32, 41, 999, seatUnit));

                        ctx.SaveChanges();
                    }
                }

                var vehPubVol2Codes = new List<string> { "630", "E63" };
                foreach (var vehVolCode in vehPubVol2Codes)
                {
                    var veh = ctx.VehicleTypeVoluntaries
                        .Include(vv => vv.VehicleTypeCompulsories)
                        .Single(x => x.Code == vehVolCode);

                    // CC
                    var com33 = veh.VehicleTypeCompulsories.SingleOrDefault(x => x.Code == "3.30");
                    if (com33 != null)
                    {
                        ctx.Add(new VehicleCompulsoryFeature(informMotoType, veh, com33, 0, 75, ccUnit));
                        ctx.Add(new VehicleCompulsoryFeature(informMotoType, veh, com33, 76, 125, ccUnit));
                        ctx.Add(new VehicleCompulsoryFeature(informMotoType, veh, com33, 126, 150, ccUnit));
                        ctx.Add(new VehicleCompulsoryFeature(informMotoType, veh, com33, 151, 9999, ccUnit));

                        ctx.SaveChanges();
                    }
                }
                var vehPubVol3Codes = new List<string> { "320", "327" };
                foreach (var vehVolCode in vehPubVol3Codes)
                {
                    var veh = ctx.VehicleTypeVoluntaries
                        .Include(vv => vv.VehicleTypeCompulsories)
                        .Single(x => x.Code == vehVolCode);

                    // TON
                    var com34 = veh.VehicleTypeCompulsories.SingleOrDefault(x => x.Code == "3.40");
                    if (com34 != null)
                    {
                        ctx.Add(new VehicleCompulsoryFeature(informMotoType, veh, com34, 0, 3, tonUnit));
                        ctx.Add(new VehicleCompulsoryFeature(informMotoType, veh, com34, 4, 6, tonUnit));
                        ctx.Add(new VehicleCompulsoryFeature(informMotoType, veh, com34, 7, 12, tonUnit));
                        ctx.Add(new VehicleCompulsoryFeature(informMotoType, veh, com34, 13, 999, tonUnit));

                        ctx.SaveChanges();
                    }
                }
                var vehPubVol4Codes = new List<string> { "420", "520", "540" };
                foreach (var vehVolCode in vehPubVol4Codes)
                {
                    var veh = ctx.VehicleTypeVoluntaries
                        .Include(vv => vv.VehicleTypeCompulsories)
                        .Single(x => x.Code == vehVolCode);

                    var com35 = veh.VehicleTypeCompulsories.SingleOrDefault(x => x.Code == "3.50");
                    if (com35 != null)
                    {
                        ctx.Add(new VehicleCompulsoryFeature(informMotoType, veh, com35));

                        ctx.SaveChanges();
                    }
                    var com36 = veh.VehicleTypeCompulsories.SingleOrDefault(x => x.Code == "3.60");
                    if (com36 != null)
                    {
                        ctx.Add(new VehicleCompulsoryFeature(informMotoType, veh, com36));

                        ctx.SaveChanges();
                    }
                }

                // รถส่วนบุคคลและรถที่ใช้รับจ้างหรือให้เช่า รถยนต์ป้ายแดง (การค้ารถยนต์)
                var veh801 = ctx.VehicleTypeVoluntaries
                        .Include(vv => vv.VehicleTypeCompulsories)
                        .Single(x => x.Code == "801");
                var com401 = veh801.VehicleTypeCompulsories.SingleOrDefault(x => x.Code == "4.01");
                if (com401 != null)
                {
                    ctx.Add(new VehicleCompulsoryFeature(informMotoType, veh801, com401));

                    ctx.SaveChanges();
                }

                // รถส่วนบุคคลและรถที่ใช้รับจ้างหรือให้เช่า รถใช้งานเกษตรตามกฏหมายด้วยรถยนต์
                var veh804 = ctx.VehicleTypeVoluntaries
                       .Include(vv => vv.VehicleTypeCompulsories)
                       .Single(x => x.Code == "804");
                var com406 = veh804.VehicleTypeCompulsories.SingleOrDefault(x => x.Code == "4.06");
                if (com406 != null)
                {
                    ctx.Add(new VehicleCompulsoryFeature(informMotoType, veh804, com406));

                    ctx.SaveChanges();
                }

                // รถส่วนบุคคลและรถที่ใช้รับจ้างหรือให้เช่า รถยนต์ประเภทอื่นๆ
                var veh806 = ctx.VehicleTypeVoluntaries
                      .Include(vv => vv.VehicleTypeCompulsories)
                      .Single(x => x.Code == "806");
                var com407 = veh806.VehicleTypeCompulsories.SingleOrDefault(x => x.Code == "4.07");
                if (com407 != null)
                {
                    ctx.Add(new VehicleCompulsoryFeature(informMotoType, veh806, com407));

                    ctx.SaveChanges();
                }
            });
        }

        protected void seedProducts_VehiclePriceGroupFeatures()
        {
            applyMigration("seedProducts_VehiclePriceGroupFeatures", "1.0.0", context =>
            {
                var vehPriGrpType = context
                 .ProductFeatureTypes
                 .Single(x => x.Code == ProductFeatureType.VehiclePriceGroup);

                context.Add(VehiclePriceGroupFeature.CreateBuilder(
                    "5",
                    "กลุ่มที่ 5", vehPriGrpType)
                    .WithRange(0, 700000)
                    .Build());
                context.Add(VehiclePriceGroupFeature.CreateBuilder(
                    "4",
                    "กลุ่มที่ 4", vehPriGrpType)
                    .WithRange(700001, 1000000)
                    .Build());
                context.Add(VehiclePriceGroupFeature.CreateBuilder(
                    "3",
                    "กลุ่มที่ 3", vehPriGrpType)
                    .WithRange(1000001, 1500000)
                    .Build());
                context.Add(VehiclePriceGroupFeature.CreateBuilder(
                    "2",
                    "กลุ่มที่ 2", vehPriGrpType)
                    .WithRange(1500001, 5000000)
                    .Build());
                context.Add(VehiclePriceGroupFeature.CreateBuilder(
                    "1",
                    "กลุ่มที่ 1", vehPriGrpType)
                    .WithRange(5000001, 99999999)
                    .Build());

                context.SaveChanges();
            });
        }

        protected void seedProducts_VehicleBrandModels()
        {
            applyMigration("seedProducts_VehicleBrandModels", "1.0.0", context =>
            {

                var brandConfigs = queryConfig<ExtCarBrand>(@"
SELECT [MakeCode]
      ,[Brand]
      ,[Description]
      ,[Priority]
      ,[IsActive]
      ,[CreatedBy]
      ,[CreatedDate]
      ,[ModifiedBy]
      ,[ModifiedDate]
  FROM [NewValidateDB].[dbo].[ExtCarBrand]
  WHERE IsActive ='Y'
");
                var vehBrandType = context
                    .ProductFeatureTypes
                    .Single(x => x.Code == ProductFeatureType.VehicleBrand);
                foreach (var config in brandConfigs)
                {
                    var brand = context.ProductFeatures.OfType<VehicleBrandFeature>()
                        .SingleOrDefault(x => x.Code == config.MakeCode);

                    if (brand == null)
                    {
                        brand = VehicleBrandFeature
                            .CreateBuilder(config.MakeCode, config.Brand, vehBrandType)
                            .Build();
                        context.Add(brand);
                        context.SaveChanges();
                    }
                }

                var modelConfigs = queryConfig<ExtCarModel>(@"
SELECT [MakeCode]
      ,[Family]
      ,[Model]
      ,[Description]
      ,[MotorGroup]
      ,[MTIKey]
      ,[Priority]
      ,[IsActive]
      ,[CreatedBy]
      ,[CreatedDate]
      ,[ModifiedBy]
      ,[ModifiedDate]
  FROM [NewValidateDB].[dbo].[ExtCarModel]
   WHERE IsActive ='Y'
");
                var vehModelType = context
                    .ProductFeatureTypes
                    .Single(x => x.Code == ProductFeatureType.VehicleModel);

                foreach (var config in modelConfigs)
                {
                    VehiclePriceGroupFeature? priceGroup = null;
                    if (!string.IsNullOrEmpty(config.MotorGroup)
                        && config.MotorGroup != "0")
                    {
                        priceGroup = context.ProductFeatures
                            .OfType<VehiclePriceGroupFeature>()
                            .Single(x => x.Code == config.MotorGroup);
                    }

                    var brand = context.ProductFeatures
                            .OfType<VehicleBrandFeature>()
                            .SingleOrDefault(x => x.Code == config.MakeCode);
                    if (brand == null) continue;

                    var codeModel = Code.ConvertCode(config.Family, "-");
                    var model = context.ProductFeatures
                            .OfType<VehicleModelFeature>()
                            .SingleOrDefault(x => x.Code == codeModel
                            && x.VehicleBrandFeature == brand);
                    if (model == null)
                    {
                        model = VehicleModelFeature
                            .CreateBuilder(codeModel, config.Model, vehModelType, brand)
                            .WithPriceGroup(priceGroup)
                            .WithMtiCode(config.MTIKey)
                            .Build();
                        context.Add(model);
                        context.SaveChanges();
                    }

                }
            });
        }

        protected void shouldBeCoverage(
    List<CoverageAvailability> requiredCoverageAvailabilities,
    string cocerageTypeCode,
    decimal value)
        {
            if (value > 0)
            {
                var covLevel = requiredCoverageAvailabilities
                  .Single(x => x.CoverageType.Code == cocerageTypeCode)
                  .CoverageLevel
                  ;
                if (covLevel is CoverageAmount amount)
                {
                    amount.Amount.ShouldBe(value);
                }
                else if (covLevel is CoverageLimit limit)
                {
                    limit.Amount.ShouldBe(value);
                }
            }
            else
            {
                requiredCoverageAvailabilities
                  .SingleOrDefault(x => x.CoverageType.Code == cocerageTypeCode)
                  .ShouldBe(null);
            }
        }

        protected void addCovAvailAmount(ProductsDbContext context,
            Product product,
            CoverageAvailabilityType covAvaiType,
            string covTypeCode,
            decimal amount,
            Unit amountUnit,
            CoverageBasis basis)
        {
            if (amount == 0) return;

            var coverageType = context.CoverageTypes.Single(x => x.Code == covTypeCode);
            var coverageLevelType = context.CoverageLevelTypes.Single(x => x.Code == CoverageLevelType.CoverageAmount);

            var coverageLevel = CoverageLevel.CreateBuilder()
                    .WithCoverageLevelType(coverageLevelType)
                    .WithAmount(amount)
                    .WithUnit(amountUnit)
                    .WithCoverageBasis(basis)
                    .Build();

            var requiredAvail = CoverageAvailability
                .CreateBuilder(product, covAvaiType, coverageType, coverageLevel)
                .Build();
            context.Add(requiredAvail);
        }

        protected void addCovAvailLimit(ProductsDbContext context,
            Product product,
            CoverageAvailabilityType covAvaiType,
            string covTypeCode,
            decimal amount,
            Unit amountUnit,
            CoverageBasis basis)
        {
            if (amount == 0) return;

            var coverageType = context.CoverageTypes.Single(x => x.Code == covTypeCode);
            var coverageLevelType = context.CoverageLevelTypes.Single(x => x.Code == CoverageLevelType.CoverageLimit);

            var coverageLevel = CoverageLevel.CreateBuilder()
                    .WithCoverageLevelType(coverageLevelType)
                    .WithLimit(amount)
                    .WithUnit(amountUnit)
                    .WithCoverageBasis(basis)
                    .Build();

            var requiredAvail = CoverageAvailability
                .CreateBuilder(product, covAvaiType, coverageType, coverageLevel)
                .Build();
            context.Add(requiredAvail);
        }

        private void addStandardProductFeatureAvailability(ProductsDbContext context,
            string packageCode, string vehicleCode, string compulsoryCodee,
            decimal min = 0, decimal max = 0, Unit? unit = null)
        {
            var standardAvailabilityType = context.ProductFeatureAvailabilityTypes
                    .Single(x => x.Code == ProductFeatureAvailabilityType.Standard);

            var package = context
                    .Products
                        .Include(p => p.ProductFeatureAvailabilities)
                    .Single(x => x.Code == packageCode);
            var vehCode = context.ProductFeatures
                .OfType<VehicleCompulsoryFeature>()
                .Single(x => x.VehicleTypeVoluntary.Code == vehicleCode
                 && x.VehicleTypeCompulsory.Code == compulsoryCodee
                 && x.Min == min
                 && x.Max == max
                 && x.Unit == unit);

            var newAvai = ProductFeatureAvailability.CreateBuilder(
                package, standardAvailabilityType, vehCode)
                .Build();

            context.Add(newAvai);
            context.SaveChanges();
        }

        protected void shouldBe_VehicleSize(ProductsDbContext ctx,
    VehicleFuelType vehicleFuelType,
    Unit unit,
    params string[] expectCodes)
        {
            var vehCcUsages = ctx
                .VehicleTypeVoluntaries
                .Where(x => x.VehicleSizes.Any(s => s.Unit == unit)
                    && x.VehicleFuelType == vehicleFuelType)
                .Select(x => x.Code)
                .OrderBy(x => x)
                .ToList();
            vehCcUsages.Select(x => x.Value).ShouldBe(expectCodes, Case.Insensitive);
        }

        protected void shouldBe_VehicleUsage(ProductsDbContext ctx,
            VehicleFuelType vehicleFuelType,
            VehicleUsage vehicleUsage,
            params string[] expectCodes)
        {
            var vehUsage1List = ctx.VehicleTypeVoluntaries
                .Where(x => x.VehicleUsage == vehicleUsage && x.VehicleFuelType == vehicleFuelType)
                .ToList();
            vehUsage1List.Select(x => x.Code.Value)
                .OrderBy(x => x)
                .ShouldBe(expectCodes, Case.Insensitive);
        }
    }
}
