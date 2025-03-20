using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PostService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update_Configuration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostCategory_Category_CategoryId",
                table: "PostCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_PostCategory_Post_PostId",
                table: "PostCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostCategory",
                table: "PostCategory");

            migrationBuilder.RenameTable(
                name: "PostCategory",
                newName: "PostCategoryMap");

            migrationBuilder.RenameIndex(
                name: "IX_PostCategory_PostId",
                table: "PostCategoryMap",
                newName: "IX_PostCategoryMap_PostId");

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "Post",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Category",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "Category",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostCategoryMap",
                table: "PostCategoryMap",
                columns: new[] { "CategoryId", "PostId" });

            migrationBuilder.AddForeignKey(
                name: "FK_PostCategoryMap_Category_CategoryId",
                table: "PostCategoryMap",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostCategoryMap_Post_PostId",
                table: "PostCategoryMap",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostCategoryMap_Category_CategoryId",
                table: "PostCategoryMap");

            migrationBuilder.DropForeignKey(
                name: "FK_PostCategoryMap_Post_PostId",
                table: "PostCategoryMap");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostCategoryMap",
                table: "PostCategoryMap");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Category");

            migrationBuilder.RenameTable(
                name: "PostCategoryMap",
                newName: "PostCategory");

            migrationBuilder.RenameIndex(
                name: "IX_PostCategoryMap_PostId",
                table: "PostCategory",
                newName: "IX_PostCategory_PostId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostCategory",
                table: "PostCategory",
                columns: new[] { "CategoryId", "PostId" });

            migrationBuilder.AddForeignKey(
                name: "FK_PostCategory_Category_CategoryId",
                table: "PostCategory",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostCategory_Post_PostId",
                table: "PostCategory",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
