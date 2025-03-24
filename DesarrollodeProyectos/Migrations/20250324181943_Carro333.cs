using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesarrollodeProyectos.Migrations
{
    /// <inheritdoc />
    public partial class Carro333 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Carts_CartId",
                table: "CartItems");

            migrationBuilder.AlterColumn<Guid>(
                name: "CartId",
                table: "CartItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CapId",
                table: "CartItems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ShirtId",
                table: "CartItems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SweaterId",
                table: "CartItems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CapId",
                table: "CartItems",
                column: "CapId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ShirtId",
                table: "CartItems",
                column: "ShirtId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_SweaterId",
                table: "CartItems",
                column: "SweaterId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Caps_CapId",
                table: "CartItems",
                column: "CapId",
                principalTable: "Caps",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Carts_CartId",
                table: "CartItems",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Shirts_ShirtId",
                table: "CartItems",
                column: "ShirtId",
                principalTable: "Shirts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Sweaters_SweaterId",
                table: "CartItems",
                column: "SweaterId",
                principalTable: "Sweaters",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Caps_CapId",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Carts_CartId",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Shirts_ShirtId",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Sweaters_SweaterId",
                table: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_CapId",
                table: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_ShirtId",
                table: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_SweaterId",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "CapId",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "ShirtId",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "SweaterId",
                table: "CartItems");

            migrationBuilder.AlterColumn<Guid>(
                name: "CartId",
                table: "CartItems",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Carts_CartId",
                table: "CartItems",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "Id");
        }
    }
}
