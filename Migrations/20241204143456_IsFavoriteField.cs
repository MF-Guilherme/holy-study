using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HolyStudy.Migrations
{
    /// <inheritdoc />
    public partial class IsFavoriteField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFavorite",
                table: "Themes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFavorite",
                table: "Themes");
        }
    }
}
