using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mti.Persistence.Products.Migrations
{
    /// <inheritdoc />
    public partial class initPrds3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VehiclePriceGroupFeatures",
                schema: "products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Min = table.Column<decimal>(type: "numeric", nullable: false),
                    Max = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehiclePriceGroupFeatures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VehiclePriceGroupFeatures_ProductFeatures_Id",
                        column: x => x.Id,
                        principalSchema: "products",
                        principalTable: "ProductFeatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VehicleModelFeatures",
                schema: "products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    MtiCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    VehicleBrandFeatureId = table.Column<Guid>(type: "uuid", nullable: false),
                    VehiclePriceGroupFeatureId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleModelFeatures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VehicleModelFeatures_ProductFeatures_Id",
                        column: x => x.Id,
                        principalSchema: "products",
                        principalTable: "ProductFeatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VehicleModelFeatures_VehicleBrandFeatures_VehicleBrandFeatu~",
                        column: x => x.VehicleBrandFeatureId,
                        principalSchema: "products",
                        principalTable: "VehicleBrandFeatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VehicleModelFeatures_VehiclePriceGroupFeatures_VehiclePrice~",
                        column: x => x.VehiclePriceGroupFeatureId,
                        principalSchema: "products",
                        principalTable: "VehiclePriceGroupFeatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VehicleModelFeatures_VehicleBrandId_Code",
                schema: "products",
                table: "VehicleModelFeatures",
                columns: new[] { "VehicleBrandFeatureId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VehicleModelFeatures_VehiclePriceGroupFeatureId",
                schema: "products",
                table: "VehicleModelFeatures",
                column: "VehiclePriceGroupFeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_VehiclePriceGroupFeatures_Code",
                schema: "products",
                table: "VehiclePriceGroupFeatures",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VehicleModelFeatures",
                schema: "products");

            migrationBuilder.DropTable(
                name: "VehiclePriceGroupFeatures",
                schema: "products");
        }
    }
}
