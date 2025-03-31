using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PostService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Rename_PostName_To_PostTitle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Post",
                newName: "Title");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Post",
                newName: "Name");
        }
    }
}
