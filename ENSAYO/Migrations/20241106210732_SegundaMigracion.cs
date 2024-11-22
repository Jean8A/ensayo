using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ENSAYO.Migrations
{
    /// <inheritdoc />
    public partial class SegundaMigracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__ImagenHab__IdHab__46E78A0C",
                table: "ImagenHabitacion");

            migrationBuilder.AddForeignKey(
                name: "FK__ImagenHab__IdHab__46E78A0C",
                table: "ImagenHabitacion",
                column: "IdHabitacion",
                principalTable: "Habitaciones",
                principalColumn: "IdHabitacion",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__ImagenHab__IdHab__46E78A0C",
                table: "ImagenHabitacion");

            migrationBuilder.AddForeignKey(
                name: "FK__ImagenHab__IdHab__46E78A0C",
                table: "ImagenHabitacion",
                column: "IdHabitacion",
                principalTable: "Habitaciones",
                principalColumn: "IdHabitacion");
        }
    }
}
