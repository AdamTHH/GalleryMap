using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GalleryMap.Database.Migrations
{
    /// <inheritdoc />
    public partial class ImageTitle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "imagelocations",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "imagelocations");
        }
    }
}
