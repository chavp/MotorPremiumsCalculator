using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InsuranceProducts.Tests.Migrations
{
    /// <inheritdoc />
    public partial class initUnits2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UnitId",
                schema: "products",
                table: "CoverageLevels",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_CoverageLevels_UnitId",
                schema: "products",
                table: "CoverageLevels",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_CoverageLevels_Units_UnitId",
                schema: "products",
                table: "CoverageLevels",
                column: "UnitId",
                principalSchema: "products",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoverageLevels_Units_UnitId",
                schema: "products",
                table: "CoverageLevels");

            migrationBuilder.DropIndex(
                name: "IX_CoverageLevels_UnitId",
                schema: "products",
                table: "CoverageLevels");

            migrationBuilder.DropColumn(
                name: "UnitId",
                schema: "products",
                table: "CoverageLevels");
        }
    }
}
