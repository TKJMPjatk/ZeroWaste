using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZeroWaste.Migrations
{
    public partial class updatePhoto2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipePhotos_RecipeReviews_RecipeReviewId",
                table: "RecipePhotos");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipePhotos_Recipies_RecipeId",
                table: "RecipePhotos");

            migrationBuilder.AlterColumn<int>(
                name: "RecipeReviewId",
                table: "RecipePhotos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "RecipeId",
                table: "RecipePhotos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipePhotos_RecipeReviews_RecipeReviewId",
                table: "RecipePhotos");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipePhotos_Recipies_RecipeId",
                table: "RecipePhotos");

            migrationBuilder.AlterColumn<int>(
                name: "RecipeReviewId",
                table: "RecipePhotos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RecipeId",
                table: "RecipePhotos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipePhotos_RecipeReviews_RecipeReviewId",
                table: "RecipePhotos",
                column: "RecipeReviewId",
                principalTable: "RecipeReviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipePhotos_Recipies_RecipeId",
                table: "RecipePhotos",
                column: "RecipeId",
                principalTable: "Recipies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
