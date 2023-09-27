using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace otel.models.EF.Migrations
{
    /// <inheritdoc />
    public partial class ini : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TickerModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(25,9)", precision: 25, scale: 9, nullable: false),
                    AdjustPrice = table.Column<decimal>(type: "numeric(25,9)", precision: 25, scale: 9, nullable: false),
                    Size = table.Column<decimal>(type: "numeric(25,9)", precision: 25, scale: 9, nullable: false),
                    RemainSize = table.Column<decimal>(type: "numeric(25,9)", precision: 25, scale: 9, nullable: false),
                    TickerTemplateModelId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TickerModels", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TickerModels");
        }
    }
}
