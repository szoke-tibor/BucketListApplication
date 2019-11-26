using Microsoft.EntityFrameworkCore.Migrations;

namespace BucketListApplication.Migrations
{
    public partial class BucketListElementAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Completed",
                table: "Element",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Element",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ListID",
                table: "Element",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Element",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Completed",
                table: "Element");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Element");

            migrationBuilder.DropColumn(
                name: "ListID",
                table: "Element");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Element");
        }
    }
}
