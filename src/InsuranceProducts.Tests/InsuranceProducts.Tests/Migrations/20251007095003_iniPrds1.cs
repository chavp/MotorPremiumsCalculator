using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InsuranceProducts.Tests.Migrations
{
    /// <inheritdoc />
    public partial class iniPrds1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoverageTypes_CoverageLevels_CoverageLevelId",
                schema: "products",
                table: "CoverageTypes");

            migrationBuilder.DropIndex(
                name: "IX_CoverageTypes_CoverageLevelId",
                schema: "products",
                table: "CoverageTypes");

            migrationBuilder.DropColumn(
                name: "CoverageLevelId",
                schema: "products",
                table: "CoverageTypes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CoverageLevelId",
                schema: "products",
                table: "CoverageTypes",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_CoverageTypes_CoverageLevelId",
                schema: "products",
                table: "CoverageTypes",
                column: "CoverageLevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_CoverageTypes_CoverageLevels_CoverageLevelId",
                schema: "products",
                table: "CoverageTypes",
                column: "CoverageLevelId",
                principalSchema: "products",
                principalTable: "CoverageLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
