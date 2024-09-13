using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAudit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Audits",
                table: "Audits");

            migrationBuilder.RenameTable(
                name: "Audits",
                newName: "Audit");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Audit",
                table: "Audit",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Audit",
                table: "Audit");

            migrationBuilder.RenameTable(
                name: "Audit",
                newName: "Audits");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Audits",
                table: "Audits",
                column: "Id");
        }
    }
}
