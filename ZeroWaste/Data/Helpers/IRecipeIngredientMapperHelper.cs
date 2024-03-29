﻿using ZeroWaste.Data.ViewModels;
using ZeroWaste.Data.ViewModels.RecipeIngredients;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Helpers
{
    public interface IRecipeIngredientMapperHelper
    {
        IEnumerable<ExistingIngredient> Map(IEnumerable<Ingredient> ingredient);
        IEnumerable<ExistingIngredient> MapHarmless(IEnumerable<HarmlessIngredient> harmlessIngredients);
    }
}
