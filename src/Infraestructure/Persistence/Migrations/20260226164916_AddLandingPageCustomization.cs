using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddLandingPageCustomization : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LandingConfigs",
                columns: table => new
                {
                    LandingConfigId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NegocioId = table.Column<int>(type: "integer", nullable: false),
                    HeroTitle = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HeroSubtitle = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    HeroImageFileName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    WhatsAppNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    WhatsAppMessage = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Activo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LandingConfigs", x => x.LandingConfigId);
                    table.ForeignKey(
                        name: "FK_LandingConfigs_Negocio_NegocioId",
                        column: x => x.NegocioId,
                        principalTable: "Negocio",
                        principalColumn: "NegocioId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LandingGalleryItems",
                columns: table => new
                {
                    LandingGalleryItemId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NegocioId = table.Column<int>(type: "integer", nullable: false),
                    LandingConfigId = table.Column<int>(type: "integer", nullable: false),
                    Orden = table.Column<int>(type: "integer", nullable: false),
                    MediaType = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    FileName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    AltText = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LandingGalleryItems", x => x.LandingGalleryItemId);
                    table.ForeignKey(
                        name: "FK_LandingGalleryItems_LandingConfigs_LandingConfigId",
                        column: x => x.LandingConfigId,
                        principalTable: "LandingConfigs",
                        principalColumn: "LandingConfigId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LandingGalleryItems_Negocio_NegocioId",
                        column: x => x.NegocioId,
                        principalTable: "Negocio",
                        principalColumn: "NegocioId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LandingServices",
                columns: table => new
                {
                    LandingServiceId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NegocioId = table.Column<int>(type: "integer", nullable: false),
                    LandingConfigId = table.Column<int>(type: "integer", nullable: false),
                    Orden = table.Column<int>(type: "integer", nullable: false),
                    IconCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Titulo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LandingServices", x => x.LandingServiceId);
                    table.ForeignKey(
                        name: "FK_LandingServices_LandingConfigs_LandingConfigId",
                        column: x => x.LandingConfigId,
                        principalTable: "LandingConfigs",
                        principalColumn: "LandingConfigId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LandingServices_Negocio_NegocioId",
                        column: x => x.NegocioId,
                        principalTable: "Negocio",
                        principalColumn: "NegocioId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LandingConfigs_NegocioId",
                table: "LandingConfigs",
                column: "NegocioId");

            migrationBuilder.CreateIndex(
                name: "IX_LandingGalleryItems_LandingConfigId",
                table: "LandingGalleryItems",
                column: "LandingConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_LandingGalleryItems_NegocioId",
                table: "LandingGalleryItems",
                column: "NegocioId");

            migrationBuilder.CreateIndex(
                name: "IX_LandingServices_LandingConfigId",
                table: "LandingServices",
                column: "LandingConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_LandingServices_NegocioId",
                table: "LandingServices",
                column: "NegocioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LandingGalleryItems");

            migrationBuilder.DropTable(
                name: "LandingServices");

            migrationBuilder.DropTable(
                name: "LandingConfigs");
        }
    }
}
