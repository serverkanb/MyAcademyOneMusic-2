using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OneMusic.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class isnew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsNewHit",
                table: "Songs");

            migrationBuilder.RenameColumn(
                name: "IsWeeklyTop",
                table: "Songs",
                newName: "IsNewSong");

            migrationBuilder.AddColumn<bool>(
                name: "IsNewAlbum",
                table: "Albums",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsNewAlbum",
                table: "Albums");

            migrationBuilder.RenameColumn(
                name: "IsNewSong",
                table: "Songs",
                newName: "IsWeeklyTop");

            migrationBuilder.AddColumn<bool>(
                name: "IsNewHit",
                table: "Songs",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
