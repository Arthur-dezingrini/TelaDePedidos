using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tela_de_pedidos.Migrations
{
    /// <inheritdoc />
    public partial class update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderValue",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "TotalValue",
                table: "Pedidos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "OrderValue",
                table: "Pedidos",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TotalValue",
                table: "Pedidos",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
