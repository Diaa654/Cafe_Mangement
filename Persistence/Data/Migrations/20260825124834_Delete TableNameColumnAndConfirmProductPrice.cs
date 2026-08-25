using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class DeleteTableNameColumnAndConfirmProductPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Tables_TableId",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "TableNumber",
                table: "Tables");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Tables_TableId",
                table: "Invoices",
                column: "TableId",
                principalTable: "Tables",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Tables_TableId",
                table: "Invoices");

            migrationBuilder.AddColumn<string>(
                name: "TableNumber",
                table: "Tables",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Tables_TableId",
                table: "Invoices",
                column: "TableId",
                principalTable: "Tables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
