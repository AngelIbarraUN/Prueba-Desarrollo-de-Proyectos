using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesarrollodeProyectos.Migrations
{
    /// <inheritdoc />
    public partial class ImagenDetalles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "OrderDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "OrderDetails");
        }
    }
}
