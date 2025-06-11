using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OneMusic.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class identity2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Albums_Singers_SingerId",
                table: "Albums");

            migrationBuilder.DropIndex(
                name: "IX_Albums_SingerId",
                table: "Albums");

            migrationBuilder.DropColumn(
                name: "SingerId",
                table: "Albums");

            migrationBuilder.AddColumn<int>(
                name: "AppUserId",
                table: "Albums",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Albums_AppUserId",
                table: "Albums",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Albums_AspNetUsers_AppUserId",
                table: "Albums",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Albums_AspNetUsers_AppUserId",
                table: "Albums");

            migrationBuilder.DropIndex(
                name: "IX_Albums_AppUserId",
                table: "Albums");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Albums");

            migrationBuilder.AddColumn<int>(
                name: "SingerId",
                table: "Albums",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Albums_SingerId",
                table: "Albums",
                column: "SingerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Albums_Singers_SingerId",
                table: "Albums",
                column: "SingerId",
                principalTable: "Singers",
                principalColumn: "SingerId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
