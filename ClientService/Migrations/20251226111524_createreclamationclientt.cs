using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReclamationService.Migrations
{
    /// <inheritdoc />
    public partial class createreclamationclientt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Reclamations",
                newName: "DateReclamation");

            migrationBuilder.AddColumn<int>(
                name: "ArticleId",
                table: "Reclamations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Prenom",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArticleId",
                table: "Reclamations");

            migrationBuilder.DropColumn(
                name: "Prenom",
                table: "Clients");

            migrationBuilder.RenameColumn(
                name: "DateReclamation",
                table: "Reclamations",
                newName: "Date");
        }
    }
}
