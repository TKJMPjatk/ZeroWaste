using ZeroWaste.Models;

namespace ZeroWaste.Data.Services.Reviews
{
    public class ReviewsService : IReviewsService
    {
        private readonly AppDbContext _context;

        public ReviewsService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddNewReturnsIdAsync(int recipeId, int stars, string description)
        {
            // TODO : Only for tests - remove!
            var ILLEGAL_CODE_TO_REMOVE = AppDbInitializer.userIds[2];
            var review = new RecipeReview()
            {
                AuthorId = ILLEGAL_CODE_TO_REMOVE,
                CreatedAt = DateTime.Now,
                Description = description,
                RecipeId = recipeId,
                Stars = stars,
            };
            await _context.RecipeReviews.AddAsync(review);
            await _context.SaveChangesAsync();
            return review.Id;
        }
    }
}
