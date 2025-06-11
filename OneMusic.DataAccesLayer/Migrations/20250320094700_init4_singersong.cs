using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OneMusic.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class init4_singersong : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SingerId",
                table: "Songs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Songs_SingerId",
                table: "Songs",
                column: "SingerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Songs_Singers_SingerId",
                table: "Songs",
                column: "SingerId",
                principalTable: "Singers",
                principalColumn: "SingerId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Songs_Singers_SingerId",
                table: "Songs");

            migrationBuilder.DropIndex(
                name: "IX_Songs_SingerId",
                table: "Songs");

            migrationBuilder.DropColumn(
                name: "SingerId",
                table: "Songs");
        }
    }
}
