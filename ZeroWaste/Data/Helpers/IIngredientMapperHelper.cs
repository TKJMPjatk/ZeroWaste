﻿using ZeroWaste.Data.ViewModels.NewIngredient;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Helpers
{
    public interface IIngredientMapperHelper
    {
        NewIngredientVM Map(Ingredient ingredient);
        Ingredient Map(NewIngredientVM ingredient);
    }
}