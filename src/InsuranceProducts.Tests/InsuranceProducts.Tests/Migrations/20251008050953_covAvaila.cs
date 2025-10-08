using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InsuranceProducts.Tests.Migrations
{
    /// <inheritdoc />
    public partial class covAvaila : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                columns: new[] { "ProductId", "CoverageAvailabilityTypeId", "CoverageTypeId", "CoverageLevelId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoverageAvailabilities",
                schema: "products");
        }
    }
}
