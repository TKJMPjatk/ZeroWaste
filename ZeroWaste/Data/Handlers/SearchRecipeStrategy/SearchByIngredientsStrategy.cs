

using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using ZeroWaste.DapperModels;
using ZeroWaste.Data.Enums;
using ZeroWaste.Data.Services.RecipesSearch;
using ZeroWaste.Data.Static;
using ZeroWaste.Data.Structs;
using ZeroWaste.Data.ViewModels;
using ZeroWaste.Data.ViewModels.RecipeSearch;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Handlers.SearchRecipeStrategy;

public class SearchByIngredientsStrategy : ISearchRecipeStrategy
{
    private readonly IRecipesSearchService _recipesSearchService;
    public SearchByIngredientsStrategy(IRecipesSearchService recipesSearchService)
    {
        _recipesSearchService = recipesSearchService;
    }
    public async Task<List<RecipeResult>> SearchRecipe(SearchRecipeResultsVm searchRecipeResultsVm)
    {
        var recipeResult = await _recipesSearchService
            .GetByIngredients(searchRecipeResultsVm.IngredientsLists, searchRecipeResultsVm.UserId);
        List<RecipeResult> newRecipesResultList = new List<RecipeResult>();
        MapToRecipeResult(recipeResult, newRecipesResultList);
        return newRecipesResultList;
    }
    private void MapToRecipeResult(List<SearchByIngredientsResults> searchByIngredientsResultsList, List<RecipeResult> recipeResultsList)
    {
        List<int> recipesIdList = searchByIngredientsResultsList
            .Select(x => x.RecipeId)
            .Distinct()
            .ToList();
        foreach (var recipe in recipesIdList)
        {
            RecipeResult recipeResult = GetSingleRecipe(recipe, searchByIngredientsResultsList);
            recipeResultsList.Add(recipeResult);
        }
    }
    private RecipeResult GetSingleRecipe(int id, List<SearchByIngredientsResults> searchByIngredientsResults)
    {
        var item = searchByIngredientsResults
            .Where(x => x.RecipeId == id)
            .Select(x => new
            {
                x.RecipeId,
                x.Title,
                x.EstimatedTime,
                x.DifficultyLevel,
                x.CategoryId,
                x.Stars
            }).FirstOrDefault();
        return new RecipeResult()
        {
            Id = item.RecipeId,
            Title = item.Title,
            EstimatedTime = item.EstimatedTime,
            DifficultyLevel = item.DifficultyLevel,
            CategoryId = item.CategoryId,
            Ingredients = GetRecipeIngredient(item.RecipeId, searchByIngredientsResults),
            Stars = item.Stars
        };
    }
    private List<string?> GetRecipeIngredient(int id, List<SearchByIngredientsResults> searchByIngredientsResultsList)
    {
        var items = searchByIngredientsResultsList
            .Where(x => x.RecipeId == id)
            .Select(x => x.IngredientName)
            .ToList();
        return items;
    }
    public SearchType GetSearchType(SearchRecipeResultsVm searchRecipeResultsVm)
    {
        return SearchType.Ingredients;
    }
}