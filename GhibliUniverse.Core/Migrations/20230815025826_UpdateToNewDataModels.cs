using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GhibliUniverse.Core.Migrations
{
    /// <inheritdoc />
    public partial class UpdateToNewDataModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name_Value",
                table: "VoiceActors");

            migrationBuilder.DropColumn(
                name: "Composer_Value",
                table: "Films");

            migrationBuilder.DropColumn(
                name: "Description_Value",
                table: "Films");

            migrationBuilder.DropColumn(
                name: "Director_Value",
                table: "Films");

            migrationBuilder.DropColumn(
                name: "Title_Value",
                table: "Films");

            migrationBuilder.RenameColumn(
                name: "Rating_Value",
                table: "Reviews",
                newName: "Rating");

            migrationBuilder.RenameColumn(
                name: "ReleaseYear_Value",
                table: "Films",
                newName: "ReleaseYear");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "VoiceActors",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Composer",
                table: "Films",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Films",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Director",
                table: "Films",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Films",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "VoiceActors");

            migrationBuilder.DropColumn(
                name: "Composer",
                table: "Films");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Films");

            migrationBuilder.DropColumn(
                name: "Director",
                table: "Films");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Films");

            migrationBuilder.RenameColumn(
                name: "Rating",
                table: "Reviews",
                newName: "Rating_Value");

            migrationBuilder.RenameColumn(
                name: "ReleaseYear",
                table: "Films",
                newName: "ReleaseYear_Value");

            migrationBuilder.AddColumn<string>(
                name: "Name_Value",
                table: "VoiceActors",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Composer_Value",
                table: "Films",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description_Value",
                table: "Films",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Director_Value",
                table: "Films",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title_Value",
                table: "Films",
                type: "text",
                nullable: true);
        }
    }
}
