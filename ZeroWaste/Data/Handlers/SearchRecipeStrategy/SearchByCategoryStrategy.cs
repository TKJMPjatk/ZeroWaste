using AutoMapper;
using ZeroWaste.DapperModels;
using ZeroWaste.Data.Enums;
using ZeroWaste.Data.Services.RecipesSearch;
using ZeroWaste.Data.Static;
using ZeroWaste.Data.Structs;
using ZeroWaste.Data.ViewModels;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Handlers.SearchRecipeStrategy;

public class SearchByCategoryStrategy : ISearchRecipeStrategy
{    private readonly IRecipesSearchService _recipesSearchService;
    public SearchByCategoryStrategy(IRecipesSearchService recipesSearchService)
    {
        _recipesSearchService = recipesSearchService;
    }
    public async Task<List<RecipeResult>> SearchRecipe(SearchRecipeResultsVm searchRecipeResultsVm)
    {
        var recipeResult = await _recipesSearchService.GetByCategoryAsync(searchRecipeResultsVm.CategoryId, searchRecipeResultsVm.UserId);
        List<RecipeResult> newRecipeResultList = new List<RecipeResult>();
        //return recipeResult.MapToRecipeResult();
        MapToRecipeResult(recipeResult, newRecipeResultList);
        return newRecipeResultList;
    }
    private void MapToRecipeResult(List<SearchByCategoryResults> searchByIngredientsResultsList, List<RecipeResult> recipeResultsList)
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
    private RecipeResult GetSingleRecipe(int id, List<SearchByCategoryResults> searchByIngredientsResults)
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
            Stars = item.Stars,
            Ingredients = GetRecipeIngredient(item.RecipeId, searchByIngredientsResults)
        };
    }    
    private List<string?> GetRecipeIngredient(int id, List<SearchByCategoryResults> searchByIngredientsResultsList)
    {
        var items = searchByIngredientsResultsList
            .Where(x => x.RecipeId == id)
            .Select(x => x.IngredientName)
            .ToList();
        return items;
    }
    public SearchType GetSearchType(SearchRecipeResultsVm searchRecipeResultsVm)
    {
        return SearchType.Categories;
    }
}