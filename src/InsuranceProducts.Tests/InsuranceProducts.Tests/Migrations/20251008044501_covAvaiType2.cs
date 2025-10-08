using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InsuranceProducts.Tests.Migrations
{
    /// <inheritdoc />
    public partial class covAvaiType2 : Migration
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
                    Code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
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
                name: "CoverageTypes",
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
                    table.PrimaryKey("PK_CoverageTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
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
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CoverageLevels",
                schema: "products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CoverageLevelTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    CoverageBasisId = table.Column<Guid>(type: "uuid", nullable: false),
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
                name: "IX_Products_Code",
                schema: "products",
                table: "Products",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoverageAmounts",
                schema: "products");

            migrationBuilder.DropTable(
                name: "CoverageAvailabilityTypes",
                schema: "products");

            migrationBuilder.DropTable(
                name: "CoverageTypes",
                schema: "products");

            migrationBuilder.DropTable(
                name: "Products",
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
