using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InsuranceProducts.Tests.Migrations
{
    /// <inheritdoc />
    public partial class iniPrds2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoverageLevels_CoverageLevelTypes_CoverageTypeId",
                schema: "products",
                table: "CoverageLevels");

            migrationBuilder.RenameColumn(
                name: "CoverageTypeId",
                schema: "products",
                table: "CoverageLevels",
                newName: "CoverageLevelTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_CoverageLevels_CoverageTypeId",
                schema: "products",
                table: "CoverageLevels",
                newName: "IX_CoverageLevels_CoverageLevelTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_CoverageLevels_CoverageLevelTypes_CoverageLevelTypeId",
                schema: "products",
                table: "CoverageLevels",
                column: "CoverageLevelTypeId",
                principalSchema: "products",
                principalTable: "CoverageLevelTypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoverageLevels_CoverageLevelTypes_CoverageLevelTypeId",
                schema: "products",
                table: "CoverageLevels");

            migrationBuilder.RenameColumn(
                name: "CoverageLevelTypeId",
                schema: "products",
                table: "CoverageLevels",
                newName: "CoverageTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_CoverageLevels_CoverageLevelTypeId",
                schema: "products",
                table: "CoverageLevels",
                newName: "IX_CoverageLevels_CoverageTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_CoverageLevels_CoverageLevelTypes_CoverageTypeId",
                schema: "products",
                table: "CoverageLevels",
                column: "CoverageTypeId",
                principalSchema: "products",
                principalTable: "CoverageLevelTypes",
                principalColumn: "Id");
        }
    }
}
