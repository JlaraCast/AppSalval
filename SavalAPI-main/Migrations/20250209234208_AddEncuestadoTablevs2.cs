using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SavalAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddEncuestadoTablevs2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Encuestado_Usuarios_IdUsuario",
                table: "Encuestado");

            migrationBuilder.DropForeignKey(
                name: "FK_Respuestas_Encuestado_IdentificacionEncuestado",
                table: "Respuestas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Encuestado",
                table: "Encuestado");

            migrationBuilder.RenameTable(
                name: "Encuestado",
                newName: "Encuestados");

            migrationBuilder.RenameIndex(
                name: "IX_Encuestado_IdUsuario",
                table: "Encuestados",
                newName: "IX_Encuestados_IdUsuario");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Encuestados",
                table: "Encuestados",
                column: "Identificacion");

            migrationBuilder.AddForeignKey(
                name: "FK_Encuestados_Usuarios_IdUsuario",
                table: "Encuestados",
                column: "IdUsuario",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Respuestas_Encuestados_IdentificacionEncuestado",
                table: "Respuestas",
                column: "IdentificacionEncuestado",
                principalTable: "Encuestados",
                principalColumn: "Identificacion",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Encuestados_Usuarios_IdUsuario",
                table: "Encuestados");

            migrationBuilder.DropForeignKey(
                name: "FK_Respuestas_Encuestados_IdentificacionEncuestado",
                table: "Respuestas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Encuestados",
                table: "Encuestados");

            migrationBuilder.RenameTable(
                name: "Encuestados",
                newName: "Encuestado");

            migrationBuilder.RenameIndex(
                name: "IX_Encuestados_IdUsuario",
                table: "Encuestado",
                newName: "IX_Encuestado_IdUsuario");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Encuestado",
                table: "Encuestado",
                column: "Identificacion");

            migrationBuilder.AddForeignKey(
                name: "FK_Encuestado_Usuarios_IdUsuario",
                table: "Encuestado",
                column: "IdUsuario",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Respuestas_Encuestado_IdentificacionEncuestado",
                table: "Respuestas",
                column: "IdentificacionEncuestado",
                principalTable: "Encuestado",
                principalColumn: "Identificacion",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
