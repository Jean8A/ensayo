using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ENSAYO.Migrations
{
    /// <inheritdoc />
    public partial class migraciondondeseagregaelinventario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Inventario",
                columns: table => new
                {
                    IdInventario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreItem = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    IdHabitacion = table.Column<int>(type: "int", nullable: true),
                    Disponible = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventario", x => x.IdInventario);
                    table.ForeignKey(
                        name: "FK_Inventario_Habitaciones_IdHabitacion",
                        column: x => x.IdHabitacion,
                        principalTable: "Habitaciones",
                        principalColumn: "IdHabitacion",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Inventario_IdHabitacion",
                table: "Inventario",
                column: "IdHabitacion");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Inventario");
        }
    }
}
