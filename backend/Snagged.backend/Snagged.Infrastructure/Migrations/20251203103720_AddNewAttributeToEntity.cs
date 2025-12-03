using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Snagged.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNewAttributeToEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsMain",
                table: "ItemImages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMain",
                table: "ItemImages");
        }
    }
}
