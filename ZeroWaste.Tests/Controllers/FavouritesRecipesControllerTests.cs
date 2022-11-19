using System.Security.Claims;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ZeroWaste.Controllers;
using ZeroWaste.Data.Services.FavouriteRecipes;

namespace ZeroWaste.Tests;

public class FavouritesRecipesControllerTests
{
    private Mock<IFavouritesRecipesService> _favouritesRecipesServiceMock;
    public FavouritesRecipesControllerTests()
    {
        _favouritesRecipesServiceMock = new Mock<IFavouritesRecipesService>();
    }
    [Fact]
    public async Task
        UnmarkFavouritesRecipes_ForExistingRecipes_ShouldReturnRedirectToActionWitSearchFavouritesActionInSearchRecipesController()
    {
        //Arrange
        var userId = Guid.NewGuid().ToString();
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId)
        }));
        var controller = new FavouritesRecipesController(_favouritesRecipesServiceMock.Object);
        controller.ControllerContext = new ControllerContext();
        controller.ControllerContext.HttpContext = new DefaultHttpContext() {User = user};
        //Act
        var result = await controller.UnmarkFavouriteRecipes(It.IsAny<int>());
        //Assert
        var viewResult = Assert.IsType<RedirectToActionResult>(result);
        viewResult.ActionName.Should().Be("SearchFavourite");
        viewResult.ControllerName.Should().Be("SearchRecipes");
    }
}