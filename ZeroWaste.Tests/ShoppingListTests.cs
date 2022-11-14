

using System.Security.Claims;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ZeroWaste.Controllers;
using ZeroWaste.Data.Handlers.ShoppingListHandlers;
using ZeroWaste.Data.Services.ShoppingLists;
using ZeroWaste.Models;

namespace ZeroWaste.Tests;

public class ShoppingListTests
{
    private Mock<IShoppingListsService> _shoppingListServiceMock;
    private Mock<IShoppingListHandler> _shoppingListHandlerMock;

    public ShoppingListTests()
    {
        _shoppingListServiceMock = new Mock<IShoppingListsService>();
        _shoppingListHandlerMock = new Mock<IShoppingListHandler>();
    }

    [Fact]
    public async Task Index_Always_ShouldReturnViewWithBunchOfShoppingList()
    {
        //Arrange
        var userId = Guid.NewGuid().ToString();
        _shoppingListServiceMock.Setup(service => service.GetByUserAsync(userId)).ReturnsAsync(GetShoppingList(userId));
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId)
        }));
        var controller = new ShoppingListsController(_shoppingListServiceMock.Object, _shoppingListHandlerMock.Object);
        controller.ControllerContext = new ControllerContext();
        controller.ControllerContext.HttpContext = new DefaultHttpContext() {User = user};
        //Act
        var result = await controller.Index();
        //Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var actionName = viewResult.ViewName;
        var model = Assert.IsAssignableFrom<List<ShoppingList>>(viewResult.ViewData.Model);
        actionName.Should().Be("Index");
        Assert.Equal(2, model.Count);
    }
    private List<ShoppingList> GetShoppingList(string userId)
    {
        var shoppingList = new List<ShoppingList>()
        {
            new ShoppingList()
            {
                Id = 1,
                Title = "Test1",
                Note = "NoteTest1",
                UserId = userId,
                CreatedAt = System.DateTime.Now
            },
            new ShoppingList()
            {
                Id = 1,
                Title = "Test2",
                Note = "NoteTest2",
                UserId = userId,
                CreatedAt = System.DateTime.Now
            }
        };
        return shoppingList;
    }
    [Fact]
    public async Task Edit_ForExistingShoppingList_ShouldReturnViewResultEditWithShoppingListModelAndViewBagHidden()
    {
        //Arrange
        _shoppingListServiceMock.Setup(service => service.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new ShoppingList()
        {
            Id = 1,
            Note = "Test note",
            Title = "Test title",
            CreatedAt = System.DateTime.Now,
            UserId = Guid.NewGuid().ToString()
        });
        var controller = new ShoppingListsController(_shoppingListServiceMock.Object, _shoppingListHandlerMock.Object);
        //Act
        var result = await controller.Edit(It.IsAny<int>());
        //Asert
        var viewResult = Assert.IsType<ViewResult>(result);
        var actionName = viewResult.ViewName;
        Assert.IsAssignableFrom<ShoppingList>(viewResult.ViewData.Model);
        actionName.Should().Be("Edit");
        Assert.Equal("hidden", controller.ViewBag.Hidden);
    }
    [Fact]
    public async Task EditTitle_ForExistingShoppingList_ShouldReturnRedirectToActionWithRouteKeyIdAndRouteValueAnyInt()
    {
        //Arrange
        _shoppingListServiceMock.Setup(service =>  service.IsShoppingListExists(It.IsAny<int>())).ReturnsAsync(true);
        var controller = new ShoppingListsController(_shoppingListServiceMock.Object, _shoppingListHandlerMock.Object);
        //Act
        var result = await controller.EditTitle(new ShoppingList());
        //Assert
        var viewResult = Assert.IsType<RedirectToActionResult>(result);
        var actionName = viewResult.ActionName;
        var routeKey = (viewResult.RouteValues.FirstOrDefault().Key);
        var routeValue = (viewResult.RouteValues.FirstOrDefault().Value);
        actionName.Should().Be("Edit");
        routeKey.Should().Be("id");
        Assert.IsType<int>(routeValue);
    }
    [Fact]
    public async Task EditTitle_ForNotExisitngShoppingList_ShouldRetunViewNotFound()
    {
        _shoppingListServiceMock.Setup(service => service.IsShoppingListExists(It.IsAny<int>())).ReturnsAsync(false);
        var controller = new ShoppingListsController(_shoppingListServiceMock.Object, _shoppingListHandlerMock.Object);
        //Act
        var result = await controller.EditTitle(new ShoppingList());
        //Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var actionName = viewResult.ViewName;
        actionName.Should().Be("NotFound");
    }
}