using ZeroWaste.Models;

namespace ZeroWaste.Data.Services.Statuses;

public interface IStatusesService
{
    Task<List<Status>> GetAllAsync();
}