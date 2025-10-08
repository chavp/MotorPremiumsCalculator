using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InsuranceProducts.Tests.Migrations
{
    /// <inheritdoc />
    public partial class initCovRangeLimit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    LimitFrom = table.Column<decimal>(type: "numeric", nullable: false),
                    LimitTo = table.Column<decimal>(type: "numeric", nullable: false)
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoverageLimits",
                schema: "products");

            migrationBuilder.DropTable(
                name: "CoverageRanges",
                schema: "products");
        }
    }
}
