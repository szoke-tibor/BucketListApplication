using Microsoft.EntityFrameworkCore.Migrations;

namespace BucketListApplication.Data.Migrations
{
    public partial class ContextCreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUsers",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                newName: "IdentityUser");

            migrationBuilder.AddColumn<string>(
                name: "About",
                table: "IdentityUser",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "IdentityUser",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfileImageURL",
                table: "IdentityUser",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "IdentityUser",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityUser",
                table: "IdentityUser",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "BucketList",
                columns: table => new
                {
                    BucketListID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    UserId1 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BucketList", x => x.BucketListID);
                    table.ForeignKey(
                        name: "FK_BucketList_IdentityUser_UserId1",
                        column: x => x.UserId1,
                        principalTable: "IdentityUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    CategoryID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.CategoryID);
                });

            migrationBuilder.CreateTable(
                name: "Design",
                columns: table => new
                {
                    DesignID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PictureURL = table.Column<string>(nullable: true),
                    BorderColorARGB = table.Column<int>(nullable: false),
                    BackgroundColorARGB = table.Column<int>(nullable: false),
                    BorderType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Design", x => x.DesignID);
                });

            migrationBuilder.CreateTable(
                name: "Element",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    DesignID = table.Column<int>(nullable: true),
                    BucketListID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Element", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Element_BucketList_BucketListID",
                        column: x => x.BucketListID,
                        principalTable: "BucketList",
                        principalColumn: "BucketListID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Element_Design_DesignID",
                        column: x => x.DesignID,
                        principalTable: "Design",
                        principalColumn: "DesignID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ElementCategory",
                columns: table => new
                {
                    ElementCategoryID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ElementID = table.Column<int>(nullable: false),
                    CategoryID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElementCategory", x => x.ElementCategoryID);
                    table.ForeignKey(
                        name: "FK_ElementCategory_Category_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Category",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ElementCategory_Element_ElementID",
                        column: x => x.ElementID,
                        principalTable: "Element",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BucketList_UserId1",
                table: "BucketList",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Element_BucketListID",
                table: "Element",
                column: "BucketListID");

            migrationBuilder.CreateIndex(
                name: "IX_Element_DesignID",
                table: "Element",
                column: "DesignID");

            migrationBuilder.CreateIndex(
                name: "IX_ElementCategory_CategoryID",
                table: "ElementCategory",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_ElementCategory_ElementID",
                table: "ElementCategory",
                column: "ElementID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_IdentityUser_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "IdentityUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_IdentityUser_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "IdentityUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_IdentityUser_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "IdentityUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_IdentityUser_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "IdentityUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_IdentityUser_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_IdentityUser_UserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_IdentityUser_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_IdentityUser_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "ElementCategory");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Element");

            migrationBuilder.DropTable(
                name: "BucketList");

            migrationBuilder.DropTable(
                name: "Design");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityUser",
                table: "IdentityUser");

            migrationBuilder.DropColumn(
                name: "About",
                table: "IdentityUser");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "IdentityUser");

            migrationBuilder.DropColumn(
                name: "ProfileImageURL",
                table: "IdentityUser");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "IdentityUser");

            migrationBuilder.RenameTable(
                name: "IdentityUser",
                newName: "AspNetUsers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUsers",
                table: "AspNetUsers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
