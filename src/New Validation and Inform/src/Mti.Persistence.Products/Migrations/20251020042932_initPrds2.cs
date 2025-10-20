using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mti.Persistence.Products.Migrations
{
    /// <inheritdoc />
    public partial class initPrds2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VehicleBrandFeatures",
                schema: "products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleBrandFeatures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VehicleBrandFeatures_ProductFeatures_Id",
                        column: x => x.Id,
                        principalSchema: "products",
                        principalTable: "ProductFeatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VehicleBrandFeatures_Code",
                schema: "products",
                table: "VehicleBrandFeatures",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VehicleBrandFeatures",
                schema: "products");
        }
    }
}
