using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MagicVilla.Migrations
{
    /// <inheritdoc />
    public partial class createdDataTableVilla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Villas",
                columns: new[] { "Id", "Capacity", "DateCreated", "DateUpdated", "Detail", "ImageUrl", "Name", "Price", "Province" },
                values: new object[,]
                {
                    { 1, 30, new DateTime(2023, 12, 3, 13, 2, 58, 335, DateTimeKind.Local).AddTicks(9048), new DateTime(2023, 12, 3, 13, 2, 58, 335, DateTimeKind.Local).AddTicks(9061), "first villa created", "", "First Villa", 550.0, "Cordoba" },
                    { 2, 60, new DateTime(2023, 12, 3, 13, 2, 58, 335, DateTimeKind.Local).AddTicks(9064), new DateTime(2023, 12, 3, 13, 2, 58, 335, DateTimeKind.Local).AddTicks(9065), "second villa created", "", "Second Villa", 350.0, "Mendoza" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
