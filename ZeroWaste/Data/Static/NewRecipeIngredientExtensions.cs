using ZeroWaste.Data.ViewModels.RecipeIngredients;

namespace ZeroWaste.Data.Static
{
    public static class NewRecipeIngredientExtensions
    {
        public static bool UserFillExistingIngredientGaps(this NewRecipeIngredient newRecipeIngredient)
        {
            return newRecipeIngredient.ExistingIngredientId > 0 && newRecipeIngredient.ExistingIngredientQuantity > 0;
        }
        public static bool UserFillNewIngredientGaps(this NewRecipeIngredient newRecipeIngredient)
        {
            return newRecipeIngredient.NewIngredientName is not null &&
                   newRecipeIngredient.NewIngredientUnitOfMeasureId > 0 &&
                   newRecipeIngredient.NewIngredientQuantity > 0 &&
                   newRecipeIngredient.NewIngredientTypeId > 0;
        }
    }
}
