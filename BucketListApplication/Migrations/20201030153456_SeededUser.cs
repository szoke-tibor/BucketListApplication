using Microsoft.EntityFrameworkCore.Migrations;

namespace BucketListApplication.Migrations
{
    public partial class SeededUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SeededUser",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SeededUser",
                table: "AspNetUsers");
        }
    }
}
