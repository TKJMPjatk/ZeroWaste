
using System.Security.Claims;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ZeroWaste.Controllers;
using ZeroWaste.Data.Handlers.AutomatedShoppingList;
using ZeroWaste.Data.ViewModels.AutomatedShoppingList;
using ZeroWaste.Models;

namespace ZeroWaste.Tests;

public class AutomatedShoppingListControllerTests
{
    private Mock<IAutomatedShoppingListHandler> _automatedShoppingListHandlerMock;
    public AutomatedShoppingListControllerTests()
    {
        _automatedShoppingListHandlerMock = new Mock<IAutomatedShoppingListHandler>();
    }
    [Fact]
    public async Task CreateFromRecipe_ForExistingRecipe_ShouldReturnCreateFromRecipeViewWithAddedShoppingListVmModel()
    {
        //Arrange
        var userId = Guid.NewGuid().ToString();
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId)
        }));
        _automatedShoppingListHandlerMock.Setup(x =>
                x.AddNewShoppingList(It.IsAny<int>(), userId))
            .ReturnsAsync(new ShoppingList());
        var controller = new AutomatedShoppingListController(_automatedShoppingListHandlerMock.Object);       
        controller.ControllerContext = new ControllerContext();
        controller.ControllerContext.HttpContext = new DefaultHttpContext() {User = user};
        //Act
        var result = await controller.CreateFromRecipe(It.IsAny<int>());
        //Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        viewResult.ViewName.Should().Be("CreateFromRecipe");
        Assert.IsType<AddedShoppingListVm>(viewResult.Model);
    }
    //Todo: Dodać dla nieistniejącego shoppingList
}