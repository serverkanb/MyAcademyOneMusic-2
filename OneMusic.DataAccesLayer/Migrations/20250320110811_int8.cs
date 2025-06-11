using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OneMusic.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class int8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AppUserId",
                table: "Singers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Singers_AppUserId",
                table: "Singers",
                column: "AppUserId",
                unique: true,
                filter: "[AppUserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Singers_AspNetUsers_AppUserId",
                table: "Singers",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Singers_AspNetUsers_AppUserId",
                table: "Singers");

            migrationBuilder.DropIndex(
                name: "IX_Singers_AppUserId",
                table: "Singers");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Singers");
        }
    }
}
