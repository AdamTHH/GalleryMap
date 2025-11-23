using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GalleryMap.Database.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "imagelocations");

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "imagelocations",
                type: "BLOB",
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "imagelocations");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "imagelocations",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
