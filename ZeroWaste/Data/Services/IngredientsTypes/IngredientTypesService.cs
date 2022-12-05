using Microsoft.EntityFrameworkCore;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Services.IngredientsTypes;

public class IngredientTypesService : IIngredientsTypesService
{
    private readonly AppDbContext _context;
    public IngredientTypesService(AppDbContext context)
    {
        _context = context;
    }
    public async Task<List<IngredientType>> GetAllAsync()
    {
        return await _context.IngredientTypes.ToListAsync();
    }
}