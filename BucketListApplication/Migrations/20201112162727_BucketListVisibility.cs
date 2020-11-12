using Microsoft.EntityFrameworkCore.Migrations;

namespace BucketListApplication.Migrations
{
    public partial class BucketListVisibility : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Visibility",
                table: "BucketList",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Visibility",
                table: "BucketList");
        }
    }
}
