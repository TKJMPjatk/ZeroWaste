using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZeroWaste.Migrations
{
    public partial class connectionAndTypos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Recipies_RecipeId",
                table: "Photos");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeIngredients_Recipies_RecipeId",
                table: "RecipeIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeReviews_Recipies_RecipeId",
                table: "RecipeReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Recipies_AspNetUsers_AuthorId",
                table: "Recipies");

            migrationBuilder.DropForeignKey(
                name: "FK_Recipies_Categories_CategoryId",
                table: "Recipies");

            migrationBuilder.DropForeignKey(
                name: "FK_Recipies_Statuses_StatusId",
                table: "Recipies");

            migrationBuilder.DropTable(
                name: "FavouriteRecipies");

            migrationBuilder.DropTable(
                name: "HatedRecipies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Recipies",
                table: "Recipies");

            migrationBuilder.RenameTable(
                name: "Recipies",
                newName: "Recipes");

            migrationBuilder.RenameIndex(
                name: "IX_Recipies_StatusId",
                table: "Recipes",
                newName: "IX_Recipes_StatusId");

            migrationBuilder.RenameIndex(
                name: "IX_Recipies_CategoryId",
                table: "Recipes",
                newName: "IX_Recipes_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Recipies_AuthorId",
                table: "Recipes",
                newName: "IX_Recipes_AuthorId");

            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "Roles",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Recipes",
                table: "Recipes",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "FavouriteRecipes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipeId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavouriteRecipes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FavouriteRecipes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_FavouriteRecipes_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "HatedRecipes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipeId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HatedRecipes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HatedRecipes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_HatedRecipes_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Roles_RoleId",
                table: "Roles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_FavouriteRecipes_RecipeId",
                table: "FavouriteRecipes",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_FavouriteRecipes_UserId",
                table: "FavouriteRecipes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_HatedRecipes_RecipeId",
                table: "HatedRecipes",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_HatedRecipes_UserId",
                table: "HatedRecipes",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_Recipes_RecipeId",
                table: "Photos",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeIngredients_Recipes_RecipeId",
                table: "RecipeIngredients",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeReviews_Recipes_RecipeId",
                table: "RecipeReviews",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_AspNetUsers_AuthorId",
                table: "Recipes",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_Categories_CategoryId",
                table: "Recipes",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_Statuses_StatusId",
                table: "Recipes",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Roles_RoleId",
                table: "Roles",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Recipes_RecipeId",
                table: "Photos");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeIngredients_Recipes_RecipeId",
                table: "RecipeIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeReviews_Recipes_RecipeId",
                table: "RecipeReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_AspNetUsers_AuthorId",
                table: "Recipes");

            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_Categories_CategoryId",
                table: "Recipes");

            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_Statuses_StatusId",
                table: "Recipes");

            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Roles_RoleId",
                table: "Roles");

            migrationBuilder.DropTable(
                name: "FavouriteRecipes");

            migrationBuilder.DropTable(
                name: "HatedRecipes");

            migrationBuilder.DropIndex(
                name: "IX_Roles_RoleId",
                table: "Roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Recipes",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Roles");

            migrationBuilder.RenameTable(
                name: "Recipes",
                newName: "Recipies");

            migrationBuilder.RenameIndex(
                name: "IX_Recipes_StatusId",
                table: "Recipies",
                newName: "IX_Recipies_StatusId");

            migrationBuilder.RenameIndex(
                name: "IX_Recipes_CategoryId",
                table: "Recipies",
                newName: "IX_Recipies_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Recipes_AuthorId",
                table: "Recipies",
                newName: "IX_Recipies_AuthorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Recipies",
                table: "Recipies",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "FavouriteRecipies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipeId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavouriteRecipies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FavouriteRecipies_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FavouriteRecipies_Recipies_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HatedRecipies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipeId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HatedRecipies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HatedRecipies_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HatedRecipies_Recipies_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FavouriteRecipies_RecipeId",
                table: "FavouriteRecipies",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_FavouriteRecipies_UserId",
                table: "FavouriteRecipies",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_HatedRecipies_RecipeId",
                table: "HatedRecipies",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_HatedRecipies_UserId",
                table: "HatedRecipies",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_Recipies_RecipeId",
                table: "Photos",
                column: "RecipeId",
                principalTable: "Recipies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeIngredients_Recipies_RecipeId",
                table: "RecipeIngredients",
                column: "RecipeId",
                principalTable: "Recipies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeReviews_Recipies_RecipeId",
                table: "RecipeReviews",
                column: "RecipeId",
                principalTable: "Recipies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Recipies_AspNetUsers_AuthorId",
                table: "Recipies",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Recipies_Categories_CategoryId",
                table: "Recipies",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Recipies_Statuses_StatusId",
                table: "Recipies",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
