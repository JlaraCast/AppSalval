using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SavalAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEncuestadoIMC : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Encuestados_Usuarios_IdUsuario",
                table: "Encuestados");

            migrationBuilder.DropIndex(
                name: "IX_Encuestados_IdUsuario",
                table: "Encuestados");

            migrationBuilder.DropColumn(
                name: "IdUsuario",
                table: "Encuestados");

            migrationBuilder.AddColumn<decimal>(
                name: "IMC",
                table: "Encuestados",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IMC",
                table: "Encuestados");

            migrationBuilder.AddColumn<int>(
                name: "IdUsuario",
                table: "Encuestados",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Encuestados_IdUsuario",
                table: "Encuestados",
                column: "IdUsuario");

            migrationBuilder.AddForeignKey(
                name: "FK_Encuestados_Usuarios_IdUsuario",
                table: "Encuestados",
                column: "IdUsuario",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
