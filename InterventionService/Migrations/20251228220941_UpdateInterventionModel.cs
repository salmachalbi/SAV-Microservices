using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InterventionService.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInterventionModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReclamationId",
                table: "Interventions",
                newName: "ClientId");

            migrationBuilder.RenameColumn(
                name: "PrixPieces",
                table: "Interventions",
                newName: "CoutPieces");

            migrationBuilder.RenameColumn(
                name: "MainOeuvre",
                table: "Interventions",
                newName: "CoutMainOeuvre");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Interventions",
                newName: "DateIntervention");

            migrationBuilder.AddColumn<bool>(
                name: "SousGarantie",
                table: "Interventions",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SousGarantie",
                table: "Interventions");

            migrationBuilder.RenameColumn(
                name: "DateIntervention",
                table: "Interventions",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "CoutPieces",
                table: "Interventions",
                newName: "PrixPieces");

            migrationBuilder.RenameColumn(
                name: "CoutMainOeuvre",
                table: "Interventions",
                newName: "MainOeuvre");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "Interventions",
                newName: "ReclamationId");
        }
    }
}
