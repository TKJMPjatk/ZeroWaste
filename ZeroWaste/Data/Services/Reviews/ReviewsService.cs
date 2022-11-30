using Microsoft.EntityFrameworkCore;
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

        public async Task<int> AddNewReturnsIdAsync(int recipeId, int stars, string description, string userId)
        {
            var review = new RecipeReview()
            {
                AuthorId = userId,
                CreatedAt = DateTime.Now,
                Description = description ?? "",
                RecipeId = recipeId,
                Stars = stars,
            };
            await _context.RecipeReviews.AddAsync(review);
            await _context.SaveChangesAsync();
            return review.Id;
        }

        public async Task<bool> ReviewExists(int recipeId, string userId)
        {
            var review = await _context.RecipeReviews.Where(r => r.AuthorId == userId && r.RecipeId == recipeId).FirstOrDefaultAsync();
            return review != null;
        }
    }
}
