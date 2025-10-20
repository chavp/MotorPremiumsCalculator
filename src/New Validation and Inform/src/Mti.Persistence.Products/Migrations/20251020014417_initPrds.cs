using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mti.Persistence.Products.Migrations
{
    /// <inheritdoc />
    public partial class initPrds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "products");

            migrationBuilder.CreateTable(
                name: "CoverageAvailabilityTypes",
                schema: "products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    CreatedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Revision = table.Column<decimal>(type: "numeric(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoverageAvailabilityTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CoverageBasises",
                schema: "products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    CreatedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Revision = table.Column<decimal>(type: "numeric(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoverageBasises", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CoverageLevelTypes",
                schema: "products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    CreatedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Revision = table.Column<decimal>(type: "numeric(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoverageLevelTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CoverageTypes",
                schema: "products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    CreatedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Revision = table.Column<decimal>(type: "numeric(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoverageTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PeriodTypes",
                schema: "products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Revision = table.Column<decimal>(type: "numeric(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeriodTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PolicyTypes",
                schema: "products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Revision = table.Column<decimal>(type: "numeric(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PolicyTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductFeatureAvailabilityTypes",
                schema: "products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    CreatedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Revision = table.Column<decimal>(type: "numeric(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductFeatureAvailabilityTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductFeatureTypes",
                schema: "products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    CreatedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Revision = table.Column<decimal>(type: "numeric(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductFeatureTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                schema: "products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    SaleStartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    SaleEndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Revision = table.Column<decimal>(type: "numeric(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UnitCategories",
                schema: "products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    CreatedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Revision = table.Column<decimal>(type: "numeric(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VehicleFuelTypes",
                schema: "products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    Prefix = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    CreatedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Revision = table.Column<decimal>(type: "numeric(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleFuelTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VehicleTypes",
                schema: "products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    CreatedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Revision = table.Column<decimal>(type: "numeric(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VehicleUsages",
                schema: "products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    CreatedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Revision = table.Column<decimal>(type: "numeric(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleUsages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VehicleWorkshopTypes",
                schema: "products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    LookupNames = table.Column<string>(type: "character varying(3000)", maxLength: 3000, nullable: true),
                    CreatedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Revision = table.Column<decimal>(type: "numeric(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleWorkshopTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CoverageTypeCompositions",
                schema: "products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FromCoverageTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    ToCoverageTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Revision = table.Column<decimal>(type: "numeric(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoverageTypeCompositions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoverageTypeCompositions_CoverageTypes_FromCoverageTypeId",
                        column: x => x.FromCoverageTypeId,
                        principalSchema: "products",
                        principalTable: "CoverageTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CoverageTypeCompositions_CoverageTypes_ToCoverageTypeId",
                        column: x => x.ToCoverageTypeId,
                        principalSchema: "products",
                        principalTable: "CoverageTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Campaigns",
                schema: "products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    PolicyTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Revision = table.Column<decimal>(type: "numeric(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campaigns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Campaigns_PolicyTypes_PolicyTypeId",
                        column: x => x.PolicyTypeId,
                        principalSchema: "products",
                        principalTable: "PolicyTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductFeatures",
                schema: "products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductFeatureTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Revision = table.Column<decimal>(type: "numeric(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductFeatures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductFeatures_ProductFeatureTypes_ProductFeatureTypeId",
                        column: x => x.ProductFeatureTypeId,
                        principalSchema: "products",
                        principalTable: "ProductFeatureTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Units",
                schema: "products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    Symbol = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: true),
                    UnitCategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Revision = table.Column<decimal>(type: "numeric(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Units_UnitCategories_UnitCategoryId",
                        column: x => x.UnitCategoryId,
                        principalSchema: "products",
                        principalTable: "UnitCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VehicleTypeVoluntaries",
                schema: "products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    VehicleFuelTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    VehicleUsageId = table.Column<Guid>(type: "uuid", nullable: false),
                    VehicleTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Revision = table.Column<decimal>(type: "numeric(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleTypeVoluntaries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VehicleTypeVoluntaries_VehicleFuelTypes_VehicleFuelTypeId",
                        column: x => x.VehicleFuelTypeId,
                        principalSchema: "products",
                        principalTable: "VehicleFuelTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VehicleTypeVoluntaries_VehicleTypes_VehicleTypeId",
                        column: x => x.VehicleTypeId,
                        principalSchema: "products",
                        principalTable: "VehicleTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VehicleTypeVoluntaries_VehicleUsages_VehicleUsageId",
                        column: x => x.VehicleUsageId,
                        principalSchema: "products",
                        principalTable: "VehicleUsages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CampaignProducts",
                schema: "products",
                columns: table => new
                {
                    CampaignId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampaignProducts", x => new { x.CampaignId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_CampaignProducts_Campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalSchema: "products",
                        principalTable: "Campaigns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CampaignProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "products",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductFeatureAvailabilities",
                schema: "products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductFeatureAvailabilityTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductFeatureId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Revision = table.Column<decimal>(type: "numeric(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductFeatureAvailabilities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductFeatureAvailabilities_ProductFeatureAvailabilityType~",
                        column: x => x.ProductFeatureAvailabilityTypeId,
                        principalSchema: "products",
                        principalTable: "ProductFeatureAvailabilityTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductFeatureAvailabilities_ProductFeatures_ProductFeature~",
                        column: x => x.ProductFeatureId,
                        principalSchema: "products",
                        principalTable: "ProductFeatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductFeatureAvailabilities_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "products",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CoverageLevels",
                schema: "products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CoverageLevelTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    CoverageBasisId = table.Column<Guid>(type: "uuid", nullable: false),
                    UnitId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Revision = table.Column<decimal>(type: "numeric(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoverageLevels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoverageLevels_CoverageBasises_CoverageBasisId",
                        column: x => x.CoverageBasisId,
                        principalSchema: "products",
                        principalTable: "CoverageBasises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CoverageLevels_CoverageLevelTypes_CoverageLevelTypeId",
                        column: x => x.CoverageLevelTypeId,
                        principalSchema: "products",
                        principalTable: "CoverageLevelTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CoverageLevels_Units_UnitId",
                        column: x => x.UnitId,
                        principalSchema: "products",
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VehicleSizes",
                schema: "products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    VehicleTypeVoluntaryId = table.Column<Guid>(type: "uuid", nullable: false),
                    Min = table.Column<decimal>(type: "numeric", nullable: false),
                    Max = table.Column<decimal>(type: "numeric", nullable: false),
                    UnitId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Revision = table.Column<decimal>(type: "numeric(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleSizes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VehicleSizes_Units_UnitId",
                        column: x => x.UnitId,
                        principalSchema: "products",
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VehicleSizes_VehicleTypeVoluntaries_VehicleTypeVoluntaryId",
                        column: x => x.VehicleTypeVoluntaryId,
                        principalSchema: "products",
                        principalTable: "VehicleTypeVoluntaries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VehicleTypeCompulsories",
                schema: "products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    VehicleTypeVoluntaryId = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    CreatedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Revision = table.Column<decimal>(type: "numeric(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleTypeCompulsories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VehicleTypeCompulsories_VehicleTypeVoluntaries_VehicleTypeV~",
                        column: x => x.VehicleTypeVoluntaryId,
                        principalSchema: "products",
                        principalTable: "VehicleTypeVoluntaries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VehicleVoluntaryFeatures",
                schema: "products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    VehicleWorkshopTypeId = table.Column<Guid>(type: "uuid", nullable: true),
                    VehicleTypeVoluntaryId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleVoluntaryFeatures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VehicleVoluntaryFeatures_ProductFeatures_Id",
                        column: x => x.Id,
                        principalSchema: "products",
                        principalTable: "ProductFeatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VehicleVoluntaryFeatures_VehicleTypeVoluntaries_VehicleType~",
                        column: x => x.VehicleTypeVoluntaryId,
                        principalSchema: "products",
                        principalTable: "VehicleTypeVoluntaries",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_VehicleVoluntaryFeatures_VehicleWorkshopTypes_VehicleWorksh~",
                        column: x => x.VehicleWorkshopTypeId,
                        principalSchema: "products",
                        principalTable: "VehicleWorkshopTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CoverageAmounts",
                schema: "products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoverageAmounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoverageAmounts_CoverageLevels_Id",
                        column: x => x.Id,
                        principalSchema: "products",
                        principalTable: "CoverageLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CoverageAvailabilities",
                schema: "products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    CoverageAvailabilityTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    CoverageTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    CoverageLevelId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Revision = table.Column<decimal>(type: "numeric(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoverageAvailabilities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoverageAvailabilities_CoverageAvailabilityTypes_CoverageAv~",
                        column: x => x.CoverageAvailabilityTypeId,
                        principalSchema: "products",
                        principalTable: "CoverageAvailabilityTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CoverageAvailabilities_CoverageLevels_CoverageLevelId",
                        column: x => x.CoverageLevelId,
                        principalSchema: "products",
                        principalTable: "CoverageLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CoverageAvailabilities_CoverageTypes_CoverageTypeId",
                        column: x => x.CoverageTypeId,
                        principalSchema: "products",
                        principalTable: "CoverageTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CoverageAvailabilities_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "products",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CoverageLimits",
                schema: "products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoverageLimits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoverageLimits_CoverageLevels_Id",
                        column: x => x.Id,
                        principalSchema: "products",
                        principalTable: "CoverageLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CoverageRanges",
                schema: "products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MinimumAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    MaximumAmount = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoverageRanges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoverageRanges_CoverageLevels_Id",
                        column: x => x.Id,
                        principalSchema: "products",
                        principalTable: "CoverageLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InsuranceRates",
                schema: "products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    CoverageTypeId = table.Column<Guid>(type: "uuid", nullable: true),
                    CoverageLevelId = table.Column<Guid>(type: "uuid", nullable: true),
                    ProductFeatureId = table.Column<Guid>(type: "uuid", nullable: true),
                    PeriodTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    UnitId = table.Column<Guid>(type: "uuid", nullable: false),
                    RateAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    EffectiveDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ExpiryDate = table.Column<DateOnly>(type: "date", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Revision = table.Column<decimal>(type: "numeric(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsuranceRates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InsuranceRates_CoverageLevels_CoverageLevelId",
                        column: x => x.CoverageLevelId,
                        principalSchema: "products",
                        principalTable: "CoverageLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InsuranceRates_CoverageTypes_CoverageTypeId",
                        column: x => x.CoverageTypeId,
                        principalSchema: "products",
                        principalTable: "CoverageTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InsuranceRates_PeriodTypes_PeriodTypeId",
                        column: x => x.PeriodTypeId,
                        principalSchema: "products",
                        principalTable: "PeriodTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InsuranceRates_ProductFeatures_ProductFeatureId",
                        column: x => x.ProductFeatureId,
                        principalSchema: "products",
                        principalTable: "ProductFeatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InsuranceRates_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "products",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InsuranceRates_Units_UnitId",
                        column: x => x.UnitId,
                        principalSchema: "products",
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VehicleCompulsoryFeatures",
                schema: "products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    VehicleTypeVoluntaryId = table.Column<Guid>(type: "uuid", nullable: true),
                    VehicleTypeCompulsoryId = table.Column<Guid>(type: "uuid", nullable: true),
                    Min = table.Column<decimal>(type: "numeric", nullable: false),
                    Max = table.Column<decimal>(type: "numeric", nullable: false),
                    UnitId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleCompulsoryFeatures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VehicleCompulsoryFeatures_ProductFeatures_Id",
                        column: x => x.Id,
                        principalSchema: "products",
                        principalTable: "ProductFeatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VehicleCompulsoryFeatures_Units_UnitId",
                        column: x => x.UnitId,
                        principalSchema: "products",
                        principalTable: "Units",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_VehicleCompulsoryFeatures_VehicleTypeCompulsories_VehicleTy~",
                        column: x => x.VehicleTypeCompulsoryId,
                        principalSchema: "products",
                        principalTable: "VehicleTypeCompulsories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_VehicleCompulsoryFeatures_VehicleTypeVoluntaries_VehicleTyp~",
                        column: x => x.VehicleTypeVoluntaryId,
                        principalSchema: "products",
                        principalTable: "VehicleTypeVoluntaries",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CampaignProducts_ProductId",
                schema: "products",
                table: "CampaignProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Campaigns_PolicyTypeId_Code",
                schema: "products",
                table: "Campaigns",
                columns: new[] { "PolicyTypeId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CoverageAvailabilities_CoverageAvailabilityTypeId",
                schema: "products",
                table: "CoverageAvailabilities",
                column: "CoverageAvailabilityTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CoverageAvailabilities_CoverageLevelId",
                schema: "products",
                table: "CoverageAvailabilities",
                column: "CoverageLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_CoverageAvailabilities_CoverageTypeId",
                schema: "products",
                table: "CoverageAvailabilities",
                column: "CoverageTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CoverageAvailabilities_ProductCoverages",
                schema: "products",
                table: "CoverageAvailabilities",
                columns: new[] { "ProductId", "CoverageAvailabilityTypeId", "CoverageTypeId", "CoverageLevelId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CoverageAvailabilityTypes_Code",
                schema: "products",
                table: "CoverageAvailabilityTypes",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CoverageBasises_Code",
                schema: "products",
                table: "CoverageBasises",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CoverageLevels_CoverageBasisId",
                schema: "products",
                table: "CoverageLevels",
                column: "CoverageBasisId");

            migrationBuilder.CreateIndex(
                name: "IX_CoverageLevels_CoverageLevelTypeId",
                schema: "products",
                table: "CoverageLevels",
                column: "CoverageLevelTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CoverageLevels_UnitId",
                schema: "products",
                table: "CoverageLevels",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_CoverageLevelTypes_Code",
                schema: "products",
                table: "CoverageLevelTypes",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CoverageTypeCompositions_FromToCoverageTypeId",
                schema: "products",
                table: "CoverageTypeCompositions",
                columns: new[] { "FromCoverageTypeId", "ToCoverageTypeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CoverageTypeCompositions_ToCoverageTypeId",
                schema: "products",
                table: "CoverageTypeCompositions",
                column: "ToCoverageTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CoverageTypes_Code",
                schema: "products",
                table: "CoverageTypes",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InsuranceRates_CoverageLevelId",
                schema: "products",
                table: "InsuranceRates",
                column: "CoverageLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_InsuranceRates_CoverageTypeId",
                schema: "products",
                table: "InsuranceRates",
                column: "CoverageTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_InsuranceRates_PeriodTypeId",
                schema: "products",
                table: "InsuranceRates",
                column: "PeriodTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_InsuranceRates_ProductFeatureId",
                schema: "products",
                table: "InsuranceRates",
                column: "ProductFeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_InsuranceRates_ProductFeatures_Coverages",
                schema: "products",
                table: "InsuranceRates",
                columns: new[] { "ProductId", "CoverageTypeId", "CoverageLevelId", "ProductFeatureId", "UnitId", "PeriodTypeId", "EffectiveDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InsuranceRates_UnitId",
                schema: "products",
                table: "InsuranceRates",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_PeriodTypes_Code",
                schema: "products",
                table: "PeriodTypes",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PolicyTypes_Code",
                schema: "products",
                table: "PolicyTypes",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductFeatureAvailabilities_ProductFeatureAvailabilityType~",
                schema: "products",
                table: "ProductFeatureAvailabilities",
                column: "ProductFeatureAvailabilityTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductFeatureAvailabilities_ProductFeatureId",
                schema: "products",
                table: "ProductFeatureAvailabilities",
                column: "ProductFeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductFeatureAvailabilities_ProductFeatures",
                schema: "products",
                table: "ProductFeatureAvailabilities",
                columns: new[] { "ProductId", "ProductFeatureAvailabilityTypeId", "ProductFeatureId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductFeatureAvailabilityTypes_Code",
                schema: "products",
                table: "ProductFeatureAvailabilityTypes",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductFeatures_ProductFeatureTypeId",
                schema: "products",
                table: "ProductFeatures",
                column: "ProductFeatureTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductFeatureTypes_Code",
                schema: "products",
                table: "ProductFeatureTypes",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_Code",
                schema: "products",
                table: "Products",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UnitCategories_Code",
                schema: "products",
                table: "UnitCategories",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Units_UnitCategoryId_Code",
                schema: "products",
                table: "Units",
                columns: new[] { "UnitCategoryId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VehicleCompulsoryFeatures_UnitId",
                schema: "products",
                table: "VehicleCompulsoryFeatures",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleCompulsoryFeatures_VehicleTypeCompulsoryId",
                schema: "products",
                table: "VehicleCompulsoryFeatures",
                column: "VehicleTypeCompulsoryId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleCompulsoryFeatures_VehicleTypeVoluntaryId_Min_UnitId_VehicleTypeCompulsoryId",
                schema: "products",
                table: "VehicleCompulsoryFeatures",
                columns: new[] { "VehicleTypeVoluntaryId", "Min", "UnitId", "VehicleTypeCompulsoryId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VehicleFuelTypes_Code",
                schema: "products",
                table: "VehicleFuelTypes",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VehicleSizes_UnitId",
                schema: "products",
                table: "VehicleSizes",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleSizes_VehicleTypeVoluntaryId",
                schema: "products",
                table: "VehicleSizes",
                column: "VehicleTypeVoluntaryId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleTypeCompulsories_VehicleTypeVoluntaryId_Code",
                schema: "products",
                table: "VehicleTypeCompulsories",
                columns: new[] { "VehicleTypeVoluntaryId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VehicleTypes_Code",
                schema: "products",
                table: "VehicleTypes",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VehicleTypeVoluntaries_Code",
                schema: "products",
                table: "VehicleTypeVoluntaries",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VehicleTypeVoluntaries_VehicleFuelTypeUsages",
                schema: "products",
                table: "VehicleTypeVoluntaries",
                columns: new[] { "VehicleFuelTypeId", "VehicleTypeId", "VehicleUsageId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VehicleTypeVoluntaries_VehicleTypeId",
                schema: "products",
                table: "VehicleTypeVoluntaries",
                column: "VehicleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleTypeVoluntaries_VehicleUsageId",
                schema: "products",
                table: "VehicleTypeVoluntaries",
                column: "VehicleUsageId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleUsages_Code",
                schema: "products",
                table: "VehicleUsages",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VehicleVoluntaryFeatures_VehicleTypeVoluntaryId",
                schema: "products",
                table: "VehicleVoluntaryFeatures",
                column: "VehicleTypeVoluntaryId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleVoluntaryFeatures_VehicleWorkshopTypeId_VehicleTypeVoluntaryId",
                schema: "products",
                table: "VehicleVoluntaryFeatures",
                columns: new[] { "VehicleWorkshopTypeId", "VehicleTypeVoluntaryId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VehicleWorkshopTypes_Code",
                schema: "products",
                table: "VehicleWorkshopTypes",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VehicleWorkshopTypes_LookupNames",
                schema: "products",
                table: "VehicleWorkshopTypes",
                column: "LookupNames");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CampaignProducts",
                schema: "products");

            migrationBuilder.DropTable(
                name: "CoverageAmounts",
                schema: "products");

            migrationBuilder.DropTable(
                name: "CoverageAvailabilities",
                schema: "products");

            migrationBuilder.DropTable(
                name: "CoverageLimits",
                schema: "products");

            migrationBuilder.DropTable(
                name: "CoverageRanges",
                schema: "products");

            migrationBuilder.DropTable(
                name: "CoverageTypeCompositions",
                schema: "products");

            migrationBuilder.DropTable(
                name: "InsuranceRates",
                schema: "products");

            migrationBuilder.DropTable(
                name: "ProductFeatureAvailabilities",
                schema: "products");

            migrationBuilder.DropTable(
                name: "VehicleCompulsoryFeatures",
                schema: "products");

            migrationBuilder.DropTable(
                name: "VehicleSizes",
                schema: "products");

            migrationBuilder.DropTable(
                name: "VehicleVoluntaryFeatures",
                schema: "products");

            migrationBuilder.DropTable(
                name: "Campaigns",
                schema: "products");

            migrationBuilder.DropTable(
                name: "CoverageAvailabilityTypes",
                schema: "products");

            migrationBuilder.DropTable(
                name: "CoverageLevels",
                schema: "products");

            migrationBuilder.DropTable(
                name: "CoverageTypes",
                schema: "products");

            migrationBuilder.DropTable(
                name: "PeriodTypes",
                schema: "products");

            migrationBuilder.DropTable(
                name: "ProductFeatureAvailabilityTypes",
                schema: "products");

            migrationBuilder.DropTable(
                name: "Products",
                schema: "products");

            migrationBuilder.DropTable(
                name: "VehicleTypeCompulsories",
                schema: "products");

            migrationBuilder.DropTable(
                name: "ProductFeatures",
                schema: "products");

            migrationBuilder.DropTable(
                name: "VehicleWorkshopTypes",
                schema: "products");

            migrationBuilder.DropTable(
                name: "PolicyTypes",
                schema: "products");

            migrationBuilder.DropTable(
                name: "CoverageBasises",
                schema: "products");

            migrationBuilder.DropTable(
                name: "CoverageLevelTypes",
                schema: "products");

            migrationBuilder.DropTable(
                name: "Units",
                schema: "products");

            migrationBuilder.DropTable(
                name: "VehicleTypeVoluntaries",
                schema: "products");

            migrationBuilder.DropTable(
                name: "ProductFeatureTypes",
                schema: "products");

            migrationBuilder.DropTable(
                name: "UnitCategories",
                schema: "products");

            migrationBuilder.DropTable(
                name: "VehicleFuelTypes",
                schema: "products");

            migrationBuilder.DropTable(
                name: "VehicleTypes",
                schema: "products");

            migrationBuilder.DropTable(
                name: "VehicleUsages",
                schema: "products");
        }
    }
}
