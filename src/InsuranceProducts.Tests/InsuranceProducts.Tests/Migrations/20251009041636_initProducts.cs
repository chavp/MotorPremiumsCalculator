using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InsuranceProducts.Tests.Migrations
{
    /// <inheritdoc />
    public partial class initProducts : Migration
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
                    LastModifiedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
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
                    LastModifiedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
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
                    LastModifiedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
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
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    CreatedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoverageTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                schema: "products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    CreatedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
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
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    CreatedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitCategories", x => x.Id);
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
                    LastModifiedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
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
                name: "Units",
                schema: "products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    Symbol = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: true),
                    UnitCategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
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
                name: "CoverageLevels",
                schema: "products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CoverageLevelTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    CoverageBasisId = table.Column<Guid>(type: "uuid", nullable: false),
                    UnitId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
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
                    LastModifiedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
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

            migrationBuilder.CreateIndex(
                name: "IX_CoverageAvailabilities_CoverageAvailabilityTypeId",
                schema: "products",
                table: "CoverageAvailabilities",
                column: "CoverageAvailabilityTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CoverageAvailabilities_CoverageLevelId",
                schema: "products",
                table: "CoverageAvailabilities",
                column: "CoverageLevelId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CoverageAvailabilities_CoverageTypeId",
                schema: "products",
                table: "CoverageAvailabilities",
                column: "CoverageTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CoverageAvailabilities_ProductCoverages",
                schema: "products",
                table: "CoverageAvailabilities",
                columns: new[] { "ProductId", "CoverageAvailabilityTypeId", "CoverageTypeId", "CoverageLevelId" });

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
                name: "IX_Units_UnitCategoryId",
                schema: "products",
                table: "Units",
                column: "UnitCategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "CoverageAvailabilityTypes",
                schema: "products");

            migrationBuilder.DropTable(
                name: "Products",
                schema: "products");

            migrationBuilder.DropTable(
                name: "CoverageLevels",
                schema: "products");

            migrationBuilder.DropTable(
                name: "CoverageTypes",
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
                name: "UnitCategories",
                schema: "products");
        }
    }
}
