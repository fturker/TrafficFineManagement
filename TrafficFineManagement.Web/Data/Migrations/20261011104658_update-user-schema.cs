using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrafficFineManagement.Web.Migrations
{
    /// <inheritdoc />
    public partial class updateuserschema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedTime",
                table: "Payments",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "CreatedTime",
                table: "Fines",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "CreatedTime",
                table: "FineHistories",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "CreatedTime",
                table: "Cars",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "LastUpdatedTaime",
                table: "AspNetUsers",
                newName: "LastUpdatedAt");

            migrationBuilder.RenameColumn(
                name: "CreatedTime",
                table: "AspNetUsers",
                newName: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TaxNumber",
                table: "AspNetUsers",
                column: "TaxNumber",
                unique: true,
                filter: "[TaxNumber] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_TaxNumber",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Payments",
                newName: "CreatedTime");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Fines",
                newName: "CreatedTime");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "FineHistories",
                newName: "CreatedTime");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Cars",
                newName: "CreatedTime");

            migrationBuilder.RenameColumn(
                name: "LastUpdatedAt",
                table: "AspNetUsers",
                newName: "LastUpdatedTaime");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "AspNetUsers",
                newName: "CreatedTime");
        }
    }
}
