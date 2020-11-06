using Microsoft.EntityFrameworkCore.Migrations;

namespace BucketListApplication.Migrations
{
    public partial class CategoryPictureFileName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PictureFileName",
                table: "Category",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PictureFileName",
                table: "Category");
        }
    }
}
