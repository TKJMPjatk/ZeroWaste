using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZeroWaste.Migrations
{
    public partial class renameTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipePhotos_RecipeReviews_RecipeReviewId",
                table: "RecipePhotos");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipePhotos_Recipies_RecipeId",
                table: "RecipePhotos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipePhotos",
                table: "RecipePhotos");

            migrationBuilder.RenameTable(
                name: "RecipePhotos",
                newName: "Photos");

            migrationBuilder.RenameIndex(
                name: "IX_RecipePhotos_RecipeReviewId",
                table: "Photos",
                newName: "IX_Photos_RecipeReviewId");

            migrationBuilder.RenameIndex(
                name: "IX_RecipePhotos_RecipeId",
                table: "Photos",
                newName: "IX_Photos_RecipeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Photos",
                table: "Photos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_RecipeReviews_RecipeReviewId",
                table: "Photos",
                column: "RecipeReviewId",
                principalTable: "RecipeReviews",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_Recipies_RecipeId",
                table: "Photos",
                column: "RecipeId",
                principalTable: "Recipies",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_RecipeReviews_RecipeReviewId",
                table: "Photos");

            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Recipies_RecipeId",
                table: "Photos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Photos",
                table: "Photos");

            migrationBuilder.RenameTable(
                name: "Photos",
                newName: "RecipePhotos");

            migrationBuilder.RenameIndex(
                name: "IX_Photos_RecipeReviewId",
                table: "RecipePhotos",
                newName: "IX_RecipePhotos_RecipeReviewId");

            migrationBuilder.RenameIndex(
                name: "IX_Photos_RecipeId",
                table: "RecipePhotos",
                newName: "IX_RecipePhotos_RecipeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipePhotos",
                table: "RecipePhotos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipePhotos_RecipeReviews_RecipeReviewId",
                table: "RecipePhotos",
                column: "RecipeReviewId",
                principalTable: "RecipeReviews",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipePhotos_Recipies_RecipeId",
                table: "RecipePhotos",
                column: "RecipeId",
                principalTable: "Recipies",
                principalColumn: "Id");
        }
    }
}
