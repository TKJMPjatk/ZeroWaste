using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ZeroWaste.Controllers;
using ZeroWaste.Data.Handlers.Account;
using ZeroWaste.Data.ViewModels.Login;
using ZeroWaste.Models;
using ZeroWaste.Tests.Helpers;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace ZeroWaste.Tests;

public class AccoutControllerTests
{
    private readonly Mock<IAccountHandler> _accountHandlerMock;
    public AccoutControllerTests()
    {
        _accountHandlerMock = new Mock<IAccountHandler>();
    }
    [Fact]
    public async Task Login_ForValidModel_ShouldReturnRedirectToActionIndexOnHomeController()
    {
        //Arrange
        _accountHandlerMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(new ApplicationUser() {Email = "email@email.com"});
        _accountHandlerMock.Setup(x => x.IsPasswordCorrectAsync(It.IsAny<ApplicationUser>(),It.IsAny<string>())).ReturnsAsync(true);
        _accountHandlerMock.Setup(x => x.SignIn(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(SignInResult.Success);
        var controller = new AccountController(_accountHandlerMock.Object);
        //Act
        var result = await controller.Login(AccountControllerViewModelsProvider.GetLoginVm());
        //Assert
        var viewResult = Assert.IsType<RedirectToActionResult>(result);
        viewResult.ActionName.Should().Be("Index");
        viewResult.ControllerName.Should().Be("Home");
    }
    [Fact]
    public async Task Login_ForInvalidModel_ShouldReturnLoginViewWithLoginVmModel()
    {
        //Arange
        var controller = new AccountController(_accountHandlerMock.Object);
        controller.ModelState.AddModelError("error","error");
        //Act
        var result = await controller.Login(AccountControllerViewModelsProvider.GetLoginVm());
        //Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        viewResult.ViewName.Should().Be("Login");
        Assert.IsType<LoginVm>(viewResult.Model);
    }
    [Fact]
    public async Task Login_ForNotExistsUser_ShouldReturnLoginViewWithModelAndErrorTempDataMessage()
    {
        //Arrange
        _accountHandlerMock
            .Setup(x => x.GetByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(null as ApplicationUser);
        var controller = new AccountController(_accountHandlerMock.Object)
        {
            TempData = ITempDataDictionaryProvider.GetTempDataDictionary()
        };
        //Act
        var result = await controller.Login(AccountControllerViewModelsProvider.GetLoginVm());
        //Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        viewResult.ViewName.Should().Be("Login");
        Assert.IsType<LoginVm>(viewResult.Model);
        viewResult.TempData["Error"].Should().Be("Niepoprawny login lub hasło. Spróbuj jeszcze raz!");
    }
    [Fact]
    public async Task Login_ForIncorrectPassword_ShouldReturnLoginViewWithModelAndErrorTempDataMessage()
    {
        //Arrange
        _accountHandlerMock
            .Setup(x => x.GetByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(new ApplicationUser() {Email = "email@email.com"});
        _accountHandlerMock
            .Setup(x => x.IsPasswordCorrectAsync(It.IsAny<ApplicationUser>(),It.IsAny<string>())).ReturnsAsync(false);
        var controller = new AccountController(_accountHandlerMock.Object)
        {
            TempData = ITempDataDictionaryProvider.GetTempDataDictionary()
        };
        //Act
        var result = await controller.Login(AccountControllerViewModelsProvider.GetLoginVm());
        //Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        viewResult.ViewName.Should().Be("Login");
        Assert.IsType<LoginVm>(viewResult.Model);
        viewResult.TempData["Error"].Should().Be("Niepoprawny login lub hasło. Spróbuj jeszcze raz!");
    }
    [Fact]
    public async Task Create_ForCorrectModel_ShouldReturnRegisterCompletedViewResult()
    {
        //Arrange
        _accountHandlerMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync(It.IsAny<ApplicationUser>());
        var controller = new AccountController(_accountHandlerMock.Object);
        //Act
        var result = await controller.Create(AccountControllerViewModelsProvider.GetRegisterVm());
        //Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        viewResult.ViewName.Should().Be("RegisterCompleted");
    }
    [Fact]
    public async Task Create_ForIncorrectModel_ShouldReturnRegisterViewWithRegisterVmModel()
    {
        //Arrange
        var controller = new AccountController(_accountHandlerMock.Object);
        controller
            .ModelState
            .AddModelError("error","error");
        //Act
        var result = await controller.Create(AccountControllerViewModelsProvider.GetRegisterVm());
        //Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        viewResult.ViewName.Should().Be("Register");
        Assert.IsType<RegisterVm>(viewResult.Model);
    }
    [Fact]
    public async Task Create_ForExistedUser_ShouldReturnViewRegisterWithRegisterVmModelAndTempDataMessage()
    {
        //Arrange
        _accountHandlerMock
            .Setup(x => x.GetByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(new ApplicationUser());
        var controller = new AccountController(_accountHandlerMock.Object)
        {
            TempData = ITempDataDictionaryProvider.GetTempDataDictionary()
        };
        //Act
        var result = await controller.Create(AccountControllerViewModelsProvider.GetRegisterVm());
        //Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.IsType<RegisterVm>(viewResult.Model);
        viewResult.TempData["error"].Should().Be("Użytkownik jest już zarejestrowany");
    }
    //Todo: Metoda change passwordą
}