using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ZeroWaste.Controllers;
using ZeroWaste.Data.Handlers.Account;
using ZeroWaste.Data.Services.Photo;
using ZeroWaste.Data.Services.Recipes;
using ZeroWaste.Data.Services.Statuses;
using ZeroWaste.Data.ViewModels;
using ZeroWaste.Data.ViewModels.ExistingRecipe;
using ZeroWaste.Data.ViewModels.NewRecepie;
using ZeroWaste.Models;

namespace ZeroWaste.Tests
{
    public class RecipesTests
    {
        private Mock<IRecipesService> _recipesServiceMock;
        private Mock<IPhotoService> _photoServiceMock;
        private Mock<IAccountHandler> _accountHandlerMock;
        private Mock<IStatusesService> _statusesServiceMock;

        public RecipesTests()
        {
            _recipesServiceMock = new Mock<IRecipesService>();
            _photoServiceMock = new Mock<IPhotoService>();
            _accountHandlerMock = new Mock<IAccountHandler>();
            _statusesServiceMock = new Mock<IStatusesService>();
        }
        [Fact]
        public async Task GetCreate_Always_ReturnsCreateViewWithCategoriesViewBag()
        {
            // Arrange
            _recipesServiceMock.Setup(c => c.GetDropdownsValuesAsync()).ReturnsAsync(GetDropdown());
            var controller = new RecipesController(_recipesServiceMock.Object, _photoServiceMock.Object, _accountHandlerMock.Object, _statusesServiceMock.Object);

            // Act
            var result = await controller.Create();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var viewDataCount = viewResult.ViewData.Count;
            viewDataCount.Should().Be(1);
            var action = viewResult.ViewName;
            action.Should().Be("Create");
        }
        [Fact]
        public async Task GetDetails_WhenRecipeNotExists_ReturnsViewNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
            new Claim(ClaimTypes.NameIdentifier, userId)
            }));
            _recipesServiceMock.Setup(c => c.GetDetailsByIdAsync(It.IsAny<int>())).ReturnsAsync(null as DetailsRecipeVM);
            var controller = new RecipesController(_recipesServiceMock.Object, _photoServiceMock.Object, _accountHandlerMock.Object, _statusesServiceMock.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            var result = await controller.Details(It.IsAny<int>(), null, null);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var action = viewResult.ViewName;
            action.Should().Be("NotFound");
        }

        [Fact]
        public async Task GetDetails_WhenRecipeNotAccepted_ReturnsViewUnauthorized()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
            new Claim(ClaimTypes.NameIdentifier, userId)
            }));
            _recipesServiceMock.Setup(c => c.GetDetailsByIdAsync(It.IsAny<int>())).ReturnsAsync(GetRejectedRecipeDetails());
            var controller = new RecipesController(_recipesServiceMock.Object, _photoServiceMock.Object, _accountHandlerMock.Object, _statusesServiceMock.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            var result = await controller.Details(It.IsAny<int>(), null, null);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var action = viewResult.ViewName;
            action.Should().Be("Unauthorized");
        }

        [Fact]
        public async Task GetDetails_WhenRecipeIsAccepted_ReturnsViewDetailsWithViewData()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
            new Claim(ClaimTypes.NameIdentifier, userId)
            }));
            _recipesServiceMock.Setup(c => c.GetDetailsByIdAsync(It.IsAny<int>())).ReturnsAsync(GetRecipeDetails());
            _statusesServiceMock.Setup(c => c.GetAllAsync()).ReturnsAsync(GetStatuses());
            var controller = new RecipesController(_recipesServiceMock.Object, _photoServiceMock.Object, _accountHandlerMock.Object, _statusesServiceMock.Object);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = user };
            string errorDataMessage = "error";
            string successDataMessage = "success";

            // Act
            var result = await controller.Details(GetRecipeDetails().Id,errorDataMessage,successDataMessage);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var action = viewResult.ViewName;
            action.Should().Be("Details");
            var viewData = viewResult.ViewData;
            viewData["Error"].Should().Be(errorDataMessage);
            viewData["Success"].Should().Be(successDataMessage);
            viewData["recipeId"].Should().Be(GetRecipeDetails().Id);
            viewData["statusName"].Should().NotBeNull();
            viewData["Statuses"].Should().NotBeNull();
        }

        [Fact]
        public async Task GetEdit_WhenRecipeIsNotFound_ReturnsViewNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            }));
            _recipesServiceMock.Setup(c => c.GetEditByIdAsync(It.IsAny<int>())).ReturnsAsync(null as EditRecipeVM);
            var controller = new RecipesController(_recipesServiceMock.Object, _photoServiceMock.Object, _accountHandlerMock.Object, _statusesServiceMock.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            var result = await controller.Edit(It.IsAny<int>());

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var action = viewResult.ViewName;
            action.Should().Be("NotFound");
        }

        [Fact]
        public async Task GetEdit_WhenUserIsNotAuthor_ReturnsViewUnauthorized()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            }));
            _recipesServiceMock.Setup(c => c.GetEditByIdAsync(It.IsAny<int>())).ReturnsAsync(new EditRecipeVM());
            _recipesServiceMock.Setup(c => c.IsAuthorEqualsEditor(It.IsAny<int>(), userId)).ReturnsAsync(false);
            var controller = new RecipesController(_recipesServiceMock.Object, _photoServiceMock.Object, _accountHandlerMock.Object, _statusesServiceMock.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
            // Act
            var result = await controller.Edit(It.IsAny<int>());

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var action = viewResult.ViewName;
            action.Should().Be("Unauthorized");
        }

        [Fact]
        public async Task GetEdit_WhenUserIsAuthor_ReturnsViewEdit()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            }));
            _recipesServiceMock.Setup(c => c.GetEditByIdAsync(It.IsAny<int>())).ReturnsAsync(new EditRecipeVM());
            _recipesServiceMock.Setup(c => c.IsAuthorEqualsEditor(It.IsAny<int>(), userId)).ReturnsAsync(true);
            _recipesServiceMock.Setup(c => c.GetDropdownsValuesAsync()).ReturnsAsync(GetDropdown());
            var controller = new RecipesController(_recipesServiceMock.Object, _photoServiceMock.Object, _accountHandlerMock.Object, _statusesServiceMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext() { User = user }
                }
            };
            // Act
            var result = await controller.Edit(It.IsAny<int>());

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var viewDataCount = viewResult.ViewData.Count;
            viewDataCount.Should().Be(1);
            var action = viewResult.ViewName;
            action.Should().Be("Edit");
        }

        [Fact]
        public async Task PostEdit_WhenModelStateIsNotValid_ReturnsEditView()
        {
            // Arrange
            _recipesServiceMock.Setup(c => c.GetDropdownsValuesAsync()).ReturnsAsync(GetDropdown());
            _photoServiceMock.Setup(c => c.GetPhotoVMAsync(It.IsAny<int>())).ReturnsAsync(new List<PhotoVM>());
            var userId = Guid.NewGuid().ToString();
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            }));
            var controller = new RecipesController(_recipesServiceMock.Object, _photoServiceMock.Object, _accountHandlerMock.Object, _statusesServiceMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext() { User = user }
                }
            };
            controller.ModelState.AddModelError("Title", "Required");
            controller.ModelState.AddModelError("Photos", "Required");
            controller.ModelState.AddModelError("NewPhotosNamesToSkip", "Required");
            controller.ModelState.AddModelError("PhotosToDelete", "Required");
            controller.ModelState.AddModelError("filesUpload", "Required");

            // Act
            var result = await controller.Edit(GetBlankObject());

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var viewDataCount = viewResult.ViewData.Count;
            viewDataCount.Should().Be(1);
            var action = viewResult.ViewName;
            action.Should().Be("Edit");
        }

        private static EditRecipeVM GetBlankObject()
        {
            var editRecipeVM = new EditRecipeVM()
            {
                Photos = new List<PhotoVM>(),
                NewPhotosNamesToSkip = "",
                PhotosToDelete = "",
                filesUpload = new List<IFormFile>()
            };
            return editRecipeVM;
        }

        private static List<Status> GetStatuses()
        {
            var statuses = new List<Status>()
            {
                new Status()
                {
                    Name = "Zatwierdzony",
                    Id = 1,
                },
                new Status()
                {
                    Name = "Niepotwierdzony",
                    Id = 2,
                },
                new Status()
                {
                    Name = "Odrzucony",
                    Id = 3,
                },
            };
            return statuses;
        }

        private static DetailsRecipeVM GetRecipeDetails()
        {
            var details = new DetailsRecipeVM()
            {
                StatusId = 1,
                Id = 1,
                Title = "test",
                EstimatedTime = 12,
                Description = "blah",
                
            };
            return details;
        }

        private static DetailsRecipeVM GetRejectedRecipeDetails()
        {
            var details = new DetailsRecipeVM()
            {
                StatusId = 2,
                Id = 1,
            };
            return details;
        }

        private static RecipeDropdownVM GetDropdown()
        {
            var dropdown = new RecipeDropdownVM()
            {
                Categories = new List<Category>()
                {
                    new Category()
                    {
                        Name = "Makarony"
                    },
                    new Category()
                    {
                        Name = "Pizza"
                    },
                    new Category()
                    {
                        Name = "Śniadania"
                    },
                }
            };
            return dropdown;
        }
    }
}
