using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Snagged.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MoveSavedForLaterToCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SavedForLater",
                table: "CartItems");

            migrationBuilder.AddColumn<bool>(
                name: "IsSavedForLater",
                table: "Carts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSavedForLater",
                table: "Carts");

            migrationBuilder.AddColumn<bool>(
                name: "SavedForLater",
                table: "CartItems",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
