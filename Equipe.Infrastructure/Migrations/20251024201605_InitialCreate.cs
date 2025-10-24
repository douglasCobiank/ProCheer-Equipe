using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Equipe.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Equipe",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NivelEquipe = table.Column<int>(type: "integer", nullable: false),
                    NonTumbling = table.Column<bool>(type: "boolean", nullable: false),
                    Modalidade = table.Column<int>(type: "integer", nullable: false),
                    TipoEquipe = table.Column<int>(type: "integer", nullable: false),
                    NomeEquipe = table.Column<string>(type: "text", nullable: false),
                    IdGinasio = table.Column<int>(type: "integer", nullable: false),
                    GinasioId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipe", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Equipe_Ginasio_GinasioId",
                        column: x => x.GinasioId,
                        principalTable: "Ginasio",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EquipeAtleta",
                columns: table => new
                {
                    IdAtleta = table.Column<int>(type: "integer", nullable: false),
                    IdEquipe = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipeAtleta", x => new { x.IdEquipe, x.IdAtleta });
                    table.ForeignKey(
                        name: "FK_EquipeAtleta_Atleta_IdAtleta",
                        column: x => x.IdAtleta,
                        principalTable: "Atleta",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EquipeAtleta_Equipe_IdEquipe",
                        column: x => x.IdEquipe,
                        principalTable: "Equipe",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Equipe_GinasioId",
                table: "Equipe",
                column: "GinasioId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipeAtleta_IdAtleta",
                table: "EquipeAtleta",
                column: "IdAtleta");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipeAtleta");

            migrationBuilder.DropTable(
                name: "Equipe");
        }
    }
}
