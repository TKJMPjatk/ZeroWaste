namespace ZeroWaste.Data.Services.Reviews
{
    public interface IReviewsService
    {
        Task<int> AddNewReturnsIdAsync(int recipeId, int stars, string description, string userId);
    }
}
