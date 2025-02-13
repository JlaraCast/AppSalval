using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SavalAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddEncuestadoRelationToRespuesta_V2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "IdentificacionEncuestado",
                table: "Respuestas",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Encuestado",
                columns: table => new
                {
                    Identificacion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TipoIdentificacion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NombreCompleto = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FechaNacimiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Altura = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Peso = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Sexo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Ubicacion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IdUsuario = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Encuestado", x => x.Identificacion);
                    table.ForeignKey(
                        name: "FK_Encuestado_Usuarios_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Respuestas_IdentificacionEncuestado",
                table: "Respuestas",
                column: "IdentificacionEncuestado");

            migrationBuilder.CreateIndex(
                name: "IX_Encuestado_IdUsuario",
                table: "Encuestado",
                column: "IdUsuario");

            migrationBuilder.AddForeignKey(
                name: "FK_Respuestas_Encuestado_IdentificacionEncuestado",
                table: "Respuestas",
                column: "IdentificacionEncuestado",
                principalTable: "Encuestado",
                principalColumn: "Identificacion",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Respuestas_Encuestado_IdentificacionEncuestado",
                table: "Respuestas");

            migrationBuilder.DropTable(
                name: "Encuestado");

            migrationBuilder.DropIndex(
                name: "IX_Respuestas_IdentificacionEncuestado",
                table: "Respuestas");

            migrationBuilder.AlterColumn<string>(
                name: "IdentificacionEncuestado",
                table: "Respuestas",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);
        }
    }
}
