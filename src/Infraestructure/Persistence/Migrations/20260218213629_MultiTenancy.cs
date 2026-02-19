using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MultiTenancy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // =====================================================
            // PASO 1: Crear tabla Negocio PRIMERO
            // =====================================================
            migrationBuilder.CreateTable(
                name: "Negocio",
                columns: table => new
                {
                    NegocioId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Slug = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    LogoUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Telefono = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Direccion = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    Correo = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    Activo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Negocio", x => x.NegocioId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Negocio_Slug",
                table: "Negocio",
                column: "Slug",
                unique: true);

            // =====================================================
            // PASO 2: Insertar negocio por defecto para datos existentes
            // =====================================================
            migrationBuilder.Sql(@"
                INSERT INTO ""Negocio"" (""Nombre"", ""Slug"", ""Descripcion"", ""Activo"", ""FechaCreacion"")
                VALUES ('Mi Negocio', 'mi-negocio', 'Negocio migrado desde datos existentes', TRUE, now());
            ");

            // =====================================================
            // PASO 3: Agregar columnas NegocioId (nullable temporal)
            // =====================================================
            migrationBuilder.AddColumn<int>(
                name: "NegocioId",
                table: "Usuario",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NegocioId",
                table: "SmtpConfigs",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NegocioId",
                table: "Servicio",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NegocioId",
                table: "Parameters",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NegocioId",
                table: "HorarioAtencion",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NegocioId",
                table: "Evento",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NegocioId",
                table: "cliente",
                type: "integer",
                nullable: true);

            // =====================================================
            // PASO 4: Asignar datos existentes al negocio por defecto
            // =====================================================
            migrationBuilder.Sql(@"
                UPDATE ""Usuario""          SET ""NegocioId"" = (SELECT ""NegocioId"" FROM ""Negocio"" WHERE ""Slug"" = 'mi-negocio' LIMIT 1);
                UPDATE ""cliente""          SET ""NegocioId"" = (SELECT ""NegocioId"" FROM ""Negocio"" WHERE ""Slug"" = 'mi-negocio' LIMIT 1);
                UPDATE ""Evento""           SET ""NegocioId"" = (SELECT ""NegocioId"" FROM ""Negocio"" WHERE ""Slug"" = 'mi-negocio' LIMIT 1);
                UPDATE ""Servicio""         SET ""NegocioId"" = (SELECT ""NegocioId"" FROM ""Negocio"" WHERE ""Slug"" = 'mi-negocio' LIMIT 1);
                UPDATE ""SmtpConfigs""      SET ""NegocioId"" = (SELECT ""NegocioId"" FROM ""Negocio"" WHERE ""Slug"" = 'mi-negocio' LIMIT 1);
                UPDATE ""HorarioAtencion""  SET ""NegocioId"" = (SELECT ""NegocioId"" FROM ""Negocio"" WHERE ""Slug"" = 'mi-negocio' LIMIT 1);
                -- Parameters se deja NULL (globales)
            ");

            // =====================================================
            // PASO 5: Hacer NOT NULL las columnas (excepto Parameters)
            // =====================================================
            migrationBuilder.AlterColumn<int>(
                name: "NegocioId",
                table: "Usuario",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "NegocioId",
                table: "SmtpConfigs",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "NegocioId",
                table: "Servicio",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "NegocioId",
                table: "HorarioAtencion",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "NegocioId",
                table: "Evento",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "NegocioId",
                table: "cliente",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            // =====================================================
            // PASO 6: Eliminar seed data de servicios (ahora se crean por negocio)
            // =====================================================
            migrationBuilder.DeleteData(
                table: "Servicio",
                keyColumn: "ServicioId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Servicio",
                keyColumn: "ServicioId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Servicio",
                keyColumn: "ServicioId",
                keyValue: 3);

            // =====================================================
            // PASO 7: Crear índices
            // =====================================================
            migrationBuilder.CreateIndex(
                name: "IX_Usuario_NegocioId",
                table: "Usuario",
                column: "NegocioId");

            migrationBuilder.CreateIndex(
                name: "IX_SmtpConfigs_NegocioId",
                table: "SmtpConfigs",
                column: "NegocioId");

            migrationBuilder.CreateIndex(
                name: "IX_Servicio_NegocioId",
                table: "Servicio",
                column: "NegocioId");

            migrationBuilder.CreateIndex(
                name: "IX_Parameters_NegocioId",
                table: "Parameters",
                column: "NegocioId");

            migrationBuilder.CreateIndex(
                name: "IX_HorarioAtencion_NegocioId",
                table: "HorarioAtencion",
                column: "NegocioId");

            migrationBuilder.CreateIndex(
                name: "IX_Evento_NegocioId",
                table: "Evento",
                column: "NegocioId");

            migrationBuilder.CreateIndex(
                name: "IX_cliente_NegocioId",
                table: "cliente",
                column: "NegocioId");

            // =====================================================
            // PASO 8: Crear foreign keys
            // =====================================================
            migrationBuilder.AddForeignKey(
                name: "FK_cliente_Negocio_NegocioId",
                table: "cliente",
                column: "NegocioId",
                principalTable: "Negocio",
                principalColumn: "NegocioId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Evento_Negocio_NegocioId",
                table: "Evento",
                column: "NegocioId",
                principalTable: "Negocio",
                principalColumn: "NegocioId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HorarioAtencion_Negocio_NegocioId",
                table: "HorarioAtencion",
                column: "NegocioId",
                principalTable: "Negocio",
                principalColumn: "NegocioId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Parameters_Negocio_NegocioId",
                table: "Parameters",
                column: "NegocioId",
                principalTable: "Negocio",
                principalColumn: "NegocioId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Servicio_Negocio_NegocioId",
                table: "Servicio",
                column: "NegocioId",
                principalTable: "Negocio",
                principalColumn: "NegocioId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SmtpConfigs_Negocio_NegocioId",
                table: "SmtpConfigs",
                column: "NegocioId",
                principalTable: "Negocio",
                principalColumn: "NegocioId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Usuario_Negocio_NegocioId",
                table: "Usuario",
                column: "NegocioId",
                principalTable: "Negocio",
                principalColumn: "NegocioId",
                onDelete: ReferentialAction.Restrict);

            // =====================================================
            // PASO 9: Crear perfil SuperAdmin si no existe
            // =====================================================
            migrationBuilder.Sql(@"
                INSERT INTO ""Perfil"" (""Nombre"", ""Rol"", ""Descripcion"")
                SELECT 'SuperAdmin', 'SuperAdmin', 'Administrador global de la plataforma multi-tenant'
                WHERE NOT EXISTS (SELECT 1 FROM ""Perfil"" WHERE ""Nombre"" = 'SuperAdmin');
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cliente_Negocio_NegocioId",
                table: "cliente");

            migrationBuilder.DropForeignKey(
                name: "FK_Evento_Negocio_NegocioId",
                table: "Evento");

            migrationBuilder.DropForeignKey(
                name: "FK_HorarioAtencion_Negocio_NegocioId",
                table: "HorarioAtencion");

            migrationBuilder.DropForeignKey(
                name: "FK_Parameters_Negocio_NegocioId",
                table: "Parameters");

            migrationBuilder.DropForeignKey(
                name: "FK_Servicio_Negocio_NegocioId",
                table: "Servicio");

            migrationBuilder.DropForeignKey(
                name: "FK_SmtpConfigs_Negocio_NegocioId",
                table: "SmtpConfigs");

            migrationBuilder.DropForeignKey(
                name: "FK_Usuario_Negocio_NegocioId",
                table: "Usuario");

            migrationBuilder.DropTable(
                name: "Negocio");

            migrationBuilder.DropIndex(
                name: "IX_Usuario_NegocioId",
                table: "Usuario");

            migrationBuilder.DropIndex(
                name: "IX_SmtpConfigs_NegocioId",
                table: "SmtpConfigs");

            migrationBuilder.DropIndex(
                name: "IX_Servicio_NegocioId",
                table: "Servicio");

            migrationBuilder.DropIndex(
                name: "IX_Parameters_NegocioId",
                table: "Parameters");

            migrationBuilder.DropIndex(
                name: "IX_HorarioAtencion_NegocioId",
                table: "HorarioAtencion");

            migrationBuilder.DropIndex(
                name: "IX_Evento_NegocioId",
                table: "Evento");

            migrationBuilder.DropIndex(
                name: "IX_cliente_NegocioId",
                table: "cliente");

            migrationBuilder.DropColumn(
                name: "NegocioId",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "NegocioId",
                table: "SmtpConfigs");

            migrationBuilder.DropColumn(
                name: "NegocioId",
                table: "Servicio");

            migrationBuilder.DropColumn(
                name: "NegocioId",
                table: "Parameters");

            migrationBuilder.DropColumn(
                name: "NegocioId",
                table: "HorarioAtencion");

            migrationBuilder.DropColumn(
                name: "NegocioId",
                table: "Evento");

            migrationBuilder.DropColumn(
                name: "NegocioId",
                table: "cliente");

            migrationBuilder.InsertData(
                table: "Servicio",
                columns: new[] { "ServicioId", "Activo", "Descripcion", "DuracionMinutos", "ImagenUrl", "Nombre", "Precio" },
                values: new object[,]
                {
                    { 1, true, "Corte de pelo profesional con estilo personalizado", 60, null, "Corte de Pelo", 25000m },
                    { 2, true, "Corte de pelo y arreglo de barba profesional", 90, null, "Corte de Pelo y Barba", 35000m },
                    { 3, true, "Arreglo y perfilado de barba profesional", 30, null, "Solo Barba", 15000m }
                });
        }
    }
}
