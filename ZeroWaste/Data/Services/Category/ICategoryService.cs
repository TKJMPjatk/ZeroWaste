using ZeroWaste.Models;

namespace ZeroWaste.Data.Services;

public interface ICategoryService
{
    Task<List<Category>> GetAllAsync();
}