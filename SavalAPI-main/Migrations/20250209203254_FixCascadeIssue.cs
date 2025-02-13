using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SavalAPI.Migrations
{
    /// <inheritdoc />
    public partial class FixCascadeIssue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FactoresRiesgo",
                columns: table => new
                {
                    IdFactor = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TextoFactor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Habilitado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FactoresRiesgo", x => x.IdFactor);
                });

            migrationBuilder.CreateTable(
                name: "Formularios",
                columns: table => new
                {
                    IdFormulario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TituloFormulario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescripcionFormulario = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Habilitado = table.Column<bool>(type: "bit", nullable: false),
                    Anonimo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Formularios", x => x.IdFormulario);
                });

            migrationBuilder.CreateTable(
                name: "Preguntas",
                columns: table => new
                {
                    IdPregunta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoPregunta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TextoPregunta = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Preguntas", x => x.IdPregunta);
                });

            migrationBuilder.CreateTable(
                name: "Recomendaciones",
                columns: table => new
                {
                    IdRecomendacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TextoRecomendacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Habilitado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recomendaciones", x => x.IdRecomendacion);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    IdRol = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreRol = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.IdRol);
                });

            migrationBuilder.CreateTable(
                name: "Respuestas",
                columns: table => new
                {
                    IdRespuesta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdFormulario = table.Column<int>(type: "int", nullable: false),
                    IdentificacionEncuestado = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaRespuesta = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Respuestas", x => x.IdRespuesta);
                    table.ForeignKey(
                        name: "FK_Respuestas_Formularios_IdFormulario",
                        column: x => x.IdFormulario,
                        principalTable: "Formularios",
                        principalColumn: "IdFormulario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FormulariosPreguntas",
                columns: table => new
                {
                    IdFormularioPregunta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdFormulario = table.Column<int>(type: "int", nullable: false),
                    IdPregunta = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormulariosPreguntas", x => x.IdFormularioPregunta);
                    table.ForeignKey(
                        name: "FK_FormulariosPreguntas_Formularios_IdFormulario",
                        column: x => x.IdFormulario,
                        principalTable: "Formularios",
                        principalColumn: "IdFormulario",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FormulariosPreguntas_Preguntas_IdPregunta",
                        column: x => x.IdPregunta,
                        principalTable: "Preguntas",
                        principalColumn: "IdPregunta",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OpcionesRespuestas",
                columns: table => new
                {
                    IdOpcion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreOpcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdPregunta = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpcionesRespuestas", x => x.IdOpcion);
                    table.ForeignKey(
                        name: "FK_OpcionesRespuestas_Preguntas_IdPregunta",
                        column: x => x.IdPregunta,
                        principalTable: "Preguntas",
                        principalColumn: "IdPregunta",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Correo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Contraseña = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdRol = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.IdUsuario);
                    table.ForeignKey(
                        name: "FK_Usuarios_Roles_IdRol",
                        column: x => x.IdRol,
                        principalTable: "Roles",
                        principalColumn: "IdRol",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DetallesRespuestas",
                columns: table => new
                {
                    IdDetalleRespuesta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdRespuesta = table.Column<int>(type: "int", nullable: false),
                    IdPregunta = table.Column<int>(type: "int", nullable: false),
                    IdOpcion = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetallesRespuestas", x => x.IdDetalleRespuesta);
                    table.ForeignKey(
                        name: "FK_DetallesRespuestas_OpcionesRespuestas_IdOpcion",
                        column: x => x.IdOpcion,
                        principalTable: "OpcionesRespuestas",
                        principalColumn: "IdOpcion",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DetallesRespuestas_Preguntas_IdPregunta",
                        column: x => x.IdPregunta,
                        principalTable: "Preguntas",
                        principalColumn: "IdPregunta",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DetallesRespuestas_Respuestas_IdRespuesta",
                        column: x => x.IdRespuesta,
                        principalTable: "Respuestas",
                        principalColumn: "IdRespuesta",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReglasOpciones",
                columns: table => new
                {
                    IdRegla = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdOpcion = table.Column<int>(type: "int", nullable: false),
                    IdRecomendacion = table.Column<int>(type: "int", nullable: true),
                    IdFactorRiesgo = table.Column<int>(type: "int", nullable: true),
                    Condicion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReglasOpciones", x => x.IdRegla);
                    table.ForeignKey(
                        name: "FK_ReglasOpciones_FactoresRiesgo_IdFactorRiesgo",
                        column: x => x.IdFactorRiesgo,
                        principalTable: "FactoresRiesgo",
                        principalColumn: "IdFactor",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ReglasOpciones_OpcionesRespuestas_IdOpcion",
                        column: x => x.IdOpcion,
                        principalTable: "OpcionesRespuestas",
                        principalColumn: "IdOpcion",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReglasOpciones_Recomendaciones_IdRecomendacion",
                        column: x => x.IdRecomendacion,
                        principalTable: "Recomendaciones",
                        principalColumn: "IdRecomendacion",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DetallesRespuestas_IdOpcion",
                table: "DetallesRespuestas",
                column: "IdOpcion");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesRespuestas_IdPregunta",
                table: "DetallesRespuestas",
                column: "IdPregunta");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesRespuestas_IdRespuesta",
                table: "DetallesRespuestas",
                column: "IdRespuesta");

            migrationBuilder.CreateIndex(
                name: "IX_FormulariosPreguntas_IdFormulario",
                table: "FormulariosPreguntas",
                column: "IdFormulario");

            migrationBuilder.CreateIndex(
                name: "IX_FormulariosPreguntas_IdPregunta",
                table: "FormulariosPreguntas",
                column: "IdPregunta");

            migrationBuilder.CreateIndex(
                name: "IX_OpcionesRespuestas_IdPregunta",
                table: "OpcionesRespuestas",
                column: "IdPregunta");

            migrationBuilder.CreateIndex(
                name: "IX_ReglasOpciones_IdFactorRiesgo",
                table: "ReglasOpciones",
                column: "IdFactorRiesgo");

            migrationBuilder.CreateIndex(
                name: "IX_ReglasOpciones_IdOpcion",
                table: "ReglasOpciones",
                column: "IdOpcion");

            migrationBuilder.CreateIndex(
                name: "IX_ReglasOpciones_IdRecomendacion",
                table: "ReglasOpciones",
                column: "IdRecomendacion");

            migrationBuilder.CreateIndex(
                name: "IX_Respuestas_IdFormulario",
                table: "Respuestas",
                column: "IdFormulario");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_IdRol",
                table: "Usuarios",
                column: "IdRol");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetallesRespuestas");

            migrationBuilder.DropTable(
                name: "FormulariosPreguntas");

            migrationBuilder.DropTable(
                name: "ReglasOpciones");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Respuestas");

            migrationBuilder.DropTable(
                name: "FactoresRiesgo");

            migrationBuilder.DropTable(
                name: "OpcionesRespuestas");

            migrationBuilder.DropTable(
                name: "Recomendaciones");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Formularios");

            migrationBuilder.DropTable(
                name: "Preguntas");
        }
    }
}
