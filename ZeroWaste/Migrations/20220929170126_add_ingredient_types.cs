﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZeroWaste.Migrations
{
    public partial class add_ingredient_types : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_IngredientType_IngredientTypeId",
                table: "Ingredients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IngredientType",
                table: "IngredientType");

            migrationBuilder.RenameTable(
                name: "IngredientType",
                newName: "IngredientTypes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IngredientTypes",
                table: "IngredientTypes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_IngredientTypes_IngredientTypeId",
                table: "Ingredients",
                column: "IngredientTypeId",
                principalTable: "IngredientTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_IngredientTypes_IngredientTypeId",
                table: "Ingredients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IngredientTypes",
                table: "IngredientTypes");

            migrationBuilder.RenameTable(
                name: "IngredientTypes",
                newName: "IngredientType");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IngredientType",
                table: "IngredientType",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_IngredientType_IngredientTypeId",
                table: "Ingredients",
                column: "IngredientTypeId",
                principalTable: "IngredientType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
