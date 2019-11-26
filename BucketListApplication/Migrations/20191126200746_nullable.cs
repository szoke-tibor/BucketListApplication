using Microsoft.EntityFrameworkCore.Migrations;

namespace BucketListApplication.Migrations
{
    public partial class nullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Element_BucketList_BucketListID",
                table: "Element");

            migrationBuilder.AddForeignKey(
                name: "FK_Element_BucketList_BucketListID",
                table: "Element",
                column: "BucketListID",
                principalTable: "BucketList",
                principalColumn: "BucketListID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Element_BucketList_BucketListID",
                table: "Element");

            migrationBuilder.AddForeignKey(
                name: "FK_Element_BucketList_BucketListID",
                table: "Element",
                column: "BucketListID",
                principalTable: "BucketList",
                principalColumn: "BucketListID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
