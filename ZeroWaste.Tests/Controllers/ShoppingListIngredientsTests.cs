using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ZeroWaste.Controllers;
using ZeroWaste.Data.Handlers.ShoppingListIngredients;
using ZeroWaste.Data.Services.ShoppingListIngredients;
using ZeroWaste.Data.Services.ShoppingLists;
using ZeroWaste.Data.ViewModels.ShoppingList;
using ZeroWaste.Data.ViewModels.ShoppingListIngredients;

namespace ZeroWaste.Tests;

public class ShoppingListIngredientsTests
{
    private Mock<IShoppingListIngredientsService> _shoppingListIngredientsServiceMock;
    private Mock<IShoppingListIngredientsHandler> _shoppingListIngredientsHandlerMock;
    public ShoppingListIngredientsTests()
    {
        _shoppingListIngredientsHandlerMock = new Mock<IShoppingListIngredientsHandler>();
        _shoppingListIngredientsServiceMock = new Mock<IShoppingListIngredientsService>();
    }
    [Fact]
    public async Task IngredientsToAdd_ForExistingShoppingList_ShouldReturnIngredientsToAddViewResultWithModelsList()
    {
        //Arrange
        _shoppingListIngredientsHandlerMock
            .Setup(x => x.GetShoppingListIngredientsVm(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(GetShoppingListIngredientsVm);
        var controller = new ShoppingListIngredientsController(_shoppingListIngredientsHandlerMock.Object);
        //Act
        var result = await controller.IngredientsToAdd(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>());
        //Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var actionName = viewResult.ViewName;
        var model = Assert.IsAssignableFrom<ShoppingListIngredientsVm>(viewResult.ViewData.Model);
        actionName.Should().Be("IngredientsToAdd");
    }
    private ShoppingListIngredientsVm GetShoppingListIngredientsVm()
    {
        return new ShoppingListIngredientsVm()
        {
            ShoppingListId = It.IsAny<int>(),
            IngredientsToAddVm = new List<IngredientsToAddVm>()
            {
                new IngredientsToAddVm() {Id = 1, Name = "Test1", IsAdded = true}
            }
        };
    }
    [Fact]
    public async Task DeleteIngredientFromShoppingList_ForExistingShoppingListIngredient_ShouldReturnRedirectToActionResultEditForControllerShoppingListWithId()
    {
        //Arrange
        _shoppingListIngredientsHandlerMock.Setup(x => x.HandleDeleteIngredientFromShoppingList(It.IsAny<int>())).ReturnsAsync(It.IsAny<int>());
        var controller = new ShoppingListIngredientsController(_shoppingListIngredientsHandlerMock.Object);
        //Act
        var result = await controller.DeleteShoppingListIngredient(It.IsAny<int>());
        var viewResult = Assert.IsType<RedirectToActionResult>(result);
        var actionName = viewResult.ActionName;
        var controllerName = viewResult.ControllerName;
        var routeName = viewResult.RouteValues.Keys.FirstOrDefault();
        var routeValue = viewResult.RouteValues.Values.FirstOrDefault();
        actionName.Should().Be("Edit");
        controllerName.Should().Be("ShoppingLists");
        routeName.Should().Be("id");
        Assert.IsType<int>(routeValue);
    }
    //Todo: What's for non existing?
    [Fact]
    public async Task AddIngredientToShoppingList_ForCorrectIds_ShouldReturnRedirectToActionIngredientsToAddResultWithIntId()
    {
        //Arrange
        var controller = new ShoppingListIngredientsController(_shoppingListIngredientsHandlerMock.Object);
        //Act
        var result = await controller.AddIngredientToShoppingList(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>());
        //Assert
        var viewResult = Assert.IsType<RedirectToActionResult>(result);
        var actionName = viewResult.ActionName;
        var routeName = viewResult.RouteValues.Keys.FirstOrDefault();
        var routeValue = viewResult.RouteValues.Values.FirstOrDefault();
        actionName.Should().Be("IngredientsToAdd");
        routeName.Should().Be("id");
        Assert.IsType<int>(routeValue);
    }

    [Fact]
    public async Task EditQuantity_ForExistingShoppingListId_ShouldReturnViewResultEditQuantityWithEditQuantityVmModel()
    {
        //Arrange
        _shoppingListIngredientsHandlerMock.Setup(x => x.GetEditQuantity(It.IsAny<int>()))
            .ReturnsAsync(new EditQuantityVM());
        var controller = new ShoppingListIngredientsController(_shoppingListIngredientsHandlerMock.Object);
        //Act
        var result = await controller.EditQuantity(It.IsAny<int>());
        //Asserts
        var viewResult = Assert.IsType<ViewResult>(result);
        var viewAction = viewResult.ViewName;
        var model = viewResult.Model;
        viewAction.Should().Be("EditQuantity");
        Assert.IsType<EditQuantityVM>(model);
    }
    //Todo: Dorobić dla nieisniejącego
    [Fact]
    public async Task PostEditQuantity_ForCorrectModel_ShouldReturnRedirectToActionEditResultForShoppingListsControllerWithIntId()
    {
        //Arrange
        var controller = new ShoppingListIngredientsController(_shoppingListIngredientsHandlerMock.Object);
        //Act
        var result = await controller.EditQuantity(new EditQuantityVM());
        //Asserts
        var viewResult = Assert.IsType<RedirectToActionResult>(result);
        var viewAction = viewResult.ActionName;
        var controllerName = viewResult.ControllerName;
        var routeName= viewResult.RouteValues.Keys.FirstOrDefault();
        var routeValue = viewResult.RouteValues.Values.FirstOrDefault();
        viewAction.Should().Be("Edit");
        controllerName.Should().Be("ShoppingLists");
        routeName.Should().Be("id");
        routeValue.Should().BeOfType<int>();
    }
    //Todo: Dla incorrect model???
    
    [Fact]
    public async Task ChangeIngredientSelection_ForExistingIngredientAndShoppingList_ShouldReturnRedirectToActionWithRouteId()
    {
        //Arrange
        var controller = new ShoppingListIngredientsController(_shoppingListIngredientsHandlerMock.Object);
        //Act
        var result = await controller.ChangeIngredientSelection(It.IsAny<int>());
        //Assert
        var viewResult = Assert.IsType<RedirectToActionResult>(result);
        var actionName = viewResult.ActionName;
        var routeKey = (viewResult.RouteValues.FirstOrDefault().Key);
        var routeValue = (viewResult.RouteValues.FirstOrDefault().Value);
        actionName.Should().Be("Edit");
        routeKey.Should().Be("id");
        Assert.IsType<int>(routeValue);
    }
    /*
    [Fact]
    public async Task ChangeIngredientSelection_ForNonExistingIngredientAndShoppingList_ShouldReturnRedirectToActionWithRouteId()
    {
        //Todo: Dokończyć   
    }
    */
}