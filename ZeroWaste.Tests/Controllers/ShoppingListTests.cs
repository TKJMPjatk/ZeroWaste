

using System.Security.Claims;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Moq;
using ZeroWaste.Controllers;
using ZeroWaste.Data.Handlers.ShoppingListHandlers;
using ZeroWaste.Data.Services.ShoppingLists;
using ZeroWaste.Data.ViewModels.ShoppingList;
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
        _shoppingListHandlerMock.Setup(service => service
                .GetShoppingListsByUserId(userId))
            .ReturnsAsync(GetShoppingList(userId));
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
        _shoppingListHandlerMock.Setup(service => service.GetShoppingListById(It.IsAny<int>()))
            .ReturnsAsync(new ShoppingList()
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
    //Todo:Jeszcze jeden test dla nie istniejącego

    [Fact]
    public async Task Delete_ForExistingShoppingList_ShouldReturnRedirectToActionWithIndexName()
    {
        //Arrange
        _shoppingListHandlerMock.Setup(x =>
            x.IsShoppingListExists(It.IsAny<int>())).ReturnsAsync(true);
        var controller = new ShoppingListsController(_shoppingListServiceMock.Object, _shoppingListHandlerMock.Object);
        //Act
        var result = await controller.Delete(It.IsAny<int>());
        //Assert
        var viewResult = Assert.IsType<RedirectToActionResult>(result);
        var actionName = viewResult.ActionName;
        actionName.Should().Be("Index");
    }
    [Fact]
    public async Task Delete_ForNonExistingShoppingList_ShouldReturnViewNotFound()
    {
        //Arrange
        _shoppingListHandlerMock.Setup(x =>
            x.IsShoppingListExists(It.IsAny<int>())).ReturnsAsync(false);
        var controller = new ShoppingListsController(_shoppingListServiceMock.Object, _shoppingListHandlerMock.Object);
        //Act
        var result = await controller.Delete(It.IsAny<int>());
        //Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var actionName = viewResult.ViewName;
        actionName.Should().Be("NotFound");
    }
    [Fact]
    public async Task Create_ForCorrectModel_ShouldReturnRedirectToActionResultWithAddedModelId()
    {   
        //Arrange
        var newShoppingListVm = new NewShoppingListVM();
        var userId = Guid.NewGuid().ToString();
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId)
        }));
        var controller = new ShoppingListsController(_shoppingListServiceMock.Object, _shoppingListHandlerMock.Object);
        controller.ControllerContext = new ControllerContext();
        controller.ControllerContext.HttpContext = new DefaultHttpContext() {User = user};
        _shoppingListHandlerMock.Setup(x => x.Create(newShoppingListVm, userId)).ReturnsAsync(new ShoppingList()
        {
            Id = It.IsAny<int>()
        });
        //Act
        var result = await controller.Create(newShoppingListVm);
        //Assert
        var viewResult = Assert.IsType<RedirectToActionResult>(result);
        var actionName = viewResult.ActionName;
        var controllerName = viewResult.ControllerName;
        var routeKey = (viewResult.RouteValues.FirstOrDefault().Key);
        var routeValue = (viewResult.RouteValues.FirstOrDefault().Value);
        controllerName.Should().Be("ShoppingListIngredients");
        actionName.Should().Be("IngredientsToAdd");
        routeKey.Should().Be("id");
        Assert.IsType<int>(routeValue);
    }
    [Fact]
    public async Task Create_ForModelStateInvalid_ShouldReturnView()
    {
        //Arrange
        var newShoppingListVm = new NewShoppingListVM();
        var userId = Guid.NewGuid().ToString();
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId)
        }));
        var controller = new ShoppingListsController(_shoppingListServiceMock.Object, _shoppingListHandlerMock.Object);
        controller.ControllerContext = new ControllerContext();
        controller.ControllerContext.HttpContext = new DefaultHttpContext() {User = user};
        controller.ModelState.AddModelError("test", "test");
        _shoppingListHandlerMock.Setup(x => x.Create(newShoppingListVm, userId)).ReturnsAsync(new ShoppingList()
        {
            Id = It.IsAny<int>()
        });
        //Act
        var result = await controller.Create(newShoppingListVm);
        //Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var actionName = viewResult.ViewName;
        var model = Assert.IsAssignableFrom<NewShoppingListVM>(viewResult.ViewData.Model);
        actionName.Should().Be("Create");
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
        //Arrange
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