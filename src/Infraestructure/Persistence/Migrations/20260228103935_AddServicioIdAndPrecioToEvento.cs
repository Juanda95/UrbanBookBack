using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddServicioIdAndPrecioToEvento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "HeroTitle",
                table: "LandingConfigs",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "HeroSubtitle",
                table: "LandingConfigs",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<decimal>(
                name: "Precio",
                table: "Evento",
                type: "numeric(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ServicioId",
                table: "Evento",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Evento_ServicioId",
                table: "Evento",
                column: "ServicioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Evento_Servicio_ServicioId",
                table: "Evento",
                column: "ServicioId",
                principalTable: "Servicio",
                principalColumn: "ServicioId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Evento_Servicio_ServicioId",
                table: "Evento");

            migrationBuilder.DropIndex(
                name: "IX_Evento_ServicioId",
                table: "Evento");

            migrationBuilder.DropColumn(
                name: "Precio",
                table: "Evento");

            migrationBuilder.DropColumn(
                name: "ServicioId",
                table: "Evento");

            migrationBuilder.AlterColumn<string>(
                name: "HeroTitle",
                table: "LandingConfigs",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "HeroSubtitle",
                table: "LandingConfigs",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true);
        }
    }
}
