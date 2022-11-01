using Microsoft.EntityFrameworkCore;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Services.Statuses;

public class StatusesService : IStatusesService
{
    private readonly AppDbContext _context;
    public StatusesService(AppDbContext context)
    {
        _context = context;
    }
    public async Task<List<Status>> GetAllAsync()
    {
        return await _context.Statuses.ToListAsync();
    }
}