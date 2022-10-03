using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZeroWaste.Migrations
{
    public partial class updatePhoto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RecipeId",
                table: "RecipePhotos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RecipePhotos_RecipeId",
                table: "RecipePhotos",
                column: "RecipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipePhotos_Recipies_RecipeId",
                table: "RecipePhotos",
                column: "RecipeId",
                principalTable: "Recipies",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipePhotos_Recipies_RecipeId",
                table: "RecipePhotos");

            migrationBuilder.DropIndex(
                name: "IX_RecipePhotos_RecipeId",
                table: "RecipePhotos");

            migrationBuilder.DropColumn(
                name: "RecipeId",
                table: "RecipePhotos");
        }
    }
}
