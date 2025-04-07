using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PostService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCascadeDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostCategoryMap_Category_CategoryId",
                table: "PostCategoryMap");

            migrationBuilder.DropForeignKey(
                name: "FK_PostCategoryMap_Post_PostId",
                table: "PostCategoryMap");

            migrationBuilder.AddForeignKey(
                name: "FK_PostCategoryMap_Category_CategoryId",
                table: "PostCategoryMap",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PostCategoryMap_Post_PostId",
                table: "PostCategoryMap",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
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
    }
}
