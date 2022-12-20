using Microsoft.EntityFrameworkCore;
using ZeroWaste.Data;
using ZeroWaste.Data.ViewModels.ShoppingList;
using ZeroWaste.Models;

namespace ZeroWaste.IntegrationTests.Helpers;

public class ShoppingListHandlerHelper
{
    internal async Task<ShoppingList> SeedShoppingList(AppDbContext context)
    {
        var shopppingList = new ShoppingList()
        {
            Note = Guid.NewGuid().ToString(),
            Title = Guid.NewGuid().ToString(),
            CreatedAt = System.DateTime.Now,
            UserId = "0d442e09-d1a4-43ea-b1da-a60a7cd0c6a0"
        };
        await context.ShoppingLists.AddAsync(shopppingList);
        await context.SaveChangesAsync();
        return shopppingList;
    }
    internal async Task<string> GetUserGuid(AppDbContext dbContext)
    {
        var userGuid = await dbContext.Users.Select(x => x.Id).FirstOrDefaultAsync();
        return userGuid;
    }
    internal async Task<int> GetShoppingListsCount(NewShoppingListVM shoppingListVm, AppDbContext dbContext)
    {
        return await dbContext
            .ShoppingLists
            .CountAsync(x =>
                x.Title == shoppingListVm.Title 
                &&  x.Note == shoppingListVm.Note);
    }
    internal async Task<int> GetShoppingListsCount(int id, AppDbContext dbContext)
    {
        return await dbContext
            .ShoppingLists
            .CountAsync(x =>
                x.Id == id);
    }
}