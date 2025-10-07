using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InsuranceProducts.Tests.Migrations
{
    /// <inheritdoc />
    public partial class iniPrds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "products");

            migrationBuilder.CreateTable(
                name: "CoverageBasises",
                schema: "products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
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
                    Code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    CreatedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoverageLevelTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CoverageLevels",
                schema: "products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CoverageTypeId = table.Column<Guid>(type: "uuid", nullable: true),
                    CoverageBasisId = table.Column<Guid>(type: "uuid", nullable: true),
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
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CoverageLevels_CoverageLevelTypes_CoverageTypeId",
                        column: x => x.CoverageTypeId,
                        principalSchema: "products",
                        principalTable: "CoverageLevelTypes",
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
                name: "CoverageTypes",
                schema: "products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    CoverageLevelId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoverageTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoverageTypes_CoverageLevels_CoverageLevelId",
                        column: x => x.CoverageLevelId,
                        principalSchema: "products",
                        principalTable: "CoverageLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "IX_CoverageLevels_CoverageTypeId",
                schema: "products",
                table: "CoverageLevels",
                column: "CoverageTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CoverageLevelTypes_Code",
                schema: "products",
                table: "CoverageLevelTypes",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CoverageTypes_Code",
                schema: "products",
                table: "CoverageTypes",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CoverageTypes_CoverageLevelId",
                schema: "products",
                table: "CoverageTypes",
                column: "CoverageLevelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoverageAmounts",
                schema: "products");

            migrationBuilder.DropTable(
                name: "CoverageTypes",
                schema: "products");

            migrationBuilder.DropTable(
                name: "CoverageLevels",
                schema: "products");

            migrationBuilder.DropTable(
                name: "CoverageBasises",
                schema: "products");

            migrationBuilder.DropTable(
                name: "CoverageLevelTypes",
                schema: "products");
        }
    }
}
