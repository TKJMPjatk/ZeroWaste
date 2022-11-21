using System.Security.Claims;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ZeroWaste.Controllers;
using ZeroWaste.Data.Services.HatedRecipes;

namespace ZeroWaste.Tests;

public class HatedRecipesControllerTests
{
    private Mock<IHatedRecipesService> _hatedRecipesServiceMock;
    public HatedRecipesControllerTests()
    {
        _hatedRecipesServiceMock = new Mock<IHatedRecipesService>();
    }
    [Fact]
    public async Task
        UnmarkHatedRecipes_ForExistingRecipes_ShouldReturnRedirectToActionResultWithActionNameSearchHatedInSearchRecipesController()
    {        
        //Arrange
        var userId = Guid.NewGuid().ToString();
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId)
        }));
        var controller = new HatedRecipesController(_hatedRecipesServiceMock.Object);
        controller.ControllerContext = new ControllerContext();
        controller.ControllerContext.HttpContext = new DefaultHttpContext() {User = user};
        //Act
        var result = await controller.UnmarkHatedRecipes(It.IsAny<int>());
        //Assert
        var viewResult = Assert.IsType<RedirectToActionResult>(result);
        viewResult.ActionName.Should().Be("SearchHated");
        viewResult.ControllerName.Should().Be("SearchRecipes");
    }
}