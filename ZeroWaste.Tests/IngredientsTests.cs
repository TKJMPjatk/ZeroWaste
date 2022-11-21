using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroWaste.Controllers;
using ZeroWaste.Data.Services;
using ZeroWaste.Data.ViewModels.NewIngredient;
using ZeroWaste.Models;

namespace ZeroWaste.Tests
{
    public class IngredientsTests
    {
        private readonly Mock<IIngredientsService> _ingredientsServiceMock;

        public IngredientsTests()
        {
            _ingredientsServiceMock = new Mock<IIngredientsService>();
        }

        //[Theory]
        //[InlineData("",40)]
        //[InlineData("ana", 2)]
        //[InlineData("p", 14)]
        //[InlineData("x", 0)]
        //[InlineData("kur", 1)]
        //[InlineData("ia", 5)]
        //[InlineData("pszen", 4)]
        //[InlineData("Mąka pszenna typ 450", 1)]
        //[InlineData("żółty", 1)]
        //[InlineData("zolty", 0)]
        //[InlineData("-", 3)]
        [Fact]
        public async Task GetIndex_WithAListOfIngredients_ReturnsAViewResult()
        {
            // Arrange
            _ingredientsServiceMock.Setup(c => c.GetAllAsync()).ReturnsAsync(GetIngredients());
            var controller = new IngredientsController(_ingredientsServiceMock.Object);

            // Act
            var result = await controller.Index("");

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Ingredient>>(viewResult.Model);
            Assert.Equal(2, model.Count());
        }
        [Fact]
        public async Task GetDetails_WhenIngredientsIsNotExists_ReturnsNotFoundResult()
        {
            // Arrange
            _ingredientsServiceMock.Setup(c => c.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(null as Ingredient);
            var controller = new IngredientsController(_ingredientsServiceMock.Object);

            // Act
            var result = await controller.Details(It.IsAny<int>());

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var action = viewResult.ViewName;
            action.Should().Be("NotFound");
        }
        [Fact]
        public async Task GetDetails_WhenIngredientsIsExists_ReturnsDetailsView()
        {
            // Arrange
            int testIngredientId = 1;
            _ingredientsServiceMock.Setup(c => c.GetByIdAsync(testIngredientId))
                .ReturnsAsync(GetIngredients().FirstOrDefault(s => s.Id == testIngredientId));
            var controller = new IngredientsController(_ingredientsServiceMock.Object);

            // Act
            var result = await controller.Details(testIngredientId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var action = viewResult.ViewName;
            var model = Assert.IsType<Ingredient>(viewResult.Model);
            model.Name.Should().Be("Papryka");
            model.Description.Should().Be("Rodzaj roślin należących do rodziny psiankowatych.");
            model.IngredientTypeId.Should().Be(3);
            //Assert.Equal(5, model.UnitOfMeasureId);
            model.UnitOfMeasureId.Should().Be(5);
            action.Should().Be("Details");
        }
        [Fact]
        public async Task GetCreate_Always_ReturnsCreateViewWithViewbags()
        {
            // Arrange
            _ingredientsServiceMock.Setup(c => c.GetNewIngredientDropdownsWM()).ReturnsAsync(GetDropdowns());
            var controller = new IngredientsController(_ingredientsServiceMock.Object);

            // Act
            var result = await controller.Create();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var viewDataCount = viewResult.ViewData.Count;
            viewDataCount.Should().Be(2);
            var action = viewResult.ViewName;
            action.Should().Be("Create");
        }
        [Fact]
        public async Task PostCreate_WhenModelIsValid_ReturnsRedirectToIndex()
        {
            // Arrange
            _ingredientsServiceMock.Setup(c => c.AddNewAsync(GetNewIngredientModel()));
            var controller = new IngredientsController(_ingredientsServiceMock.Object);

            // Act
            var result = await controller.Create(GetNewIngredientModel());

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            viewResult.ActionName.Should().Be("Index");
        }
        [Fact]
        public async Task PostCreate_WhenModelIsNotValid_ReturnsCreateViewWithViewbags()
        {
            // Arrange
            _ingredientsServiceMock.Setup(c => c.GetNewIngredientDropdownsWM()).ReturnsAsync(GetDropdowns());
            var controller = new IngredientsController(_ingredientsServiceMock.Object);
            controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await controller.Create(GetNewIngredientModel());

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var viewDataCount = viewResult.ViewData.Count;
            viewDataCount.Should().Be(2);
            var action = viewResult.ViewName;
            action.Should().Be("Create");
        }
        [Fact]
        public async Task GetEdit_WhenIngredientExists_ReturnsEditViewWithViewbags()
        {
            // Arrange
            _ingredientsServiceMock.Setup(c => c.GetNewIngredientDropdownsWM()).ReturnsAsync(GetDropdowns());
            _ingredientsServiceMock.Setup(c => c.GetVmByIdAsync(It.IsAny<int>())).ReturnsAsync(GetNewIngredientModel());
            var controller = new IngredientsController(_ingredientsServiceMock.Object);

            // Act
            var result = await controller.Edit(It.IsAny<int>());

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var viewDataCount = viewResult.ViewData.Count;
            viewDataCount.Should().Be(2);
            var action = viewResult.ViewName;
            action.Should().Be("Edit");
        }
        [Fact]
        public async Task GetEdit_WhenIngredientNotExists_ReturnsNotFoundView()
        {
            // Arrange
            var controller = new IngredientsController(_ingredientsServiceMock.Object);

            // Act
            var result = await controller.Edit(It.IsAny<int>());

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var action = viewResult.ViewName;
            action.Should().Be("NotFound");
        }
        [Fact]
        public async Task PostEdit_WhenIdsAreDifferent_ReturnsNotFoundView()
        {
            // Arrange
            int notExistingId = -1;
            _ingredientsServiceMock.Setup(c => c.GetVmByIdAsync(It.IsAny<int>())).ReturnsAsync(GetNewIngredientModel());
            var controller = new IngredientsController(_ingredientsServiceMock.Object);

            // Act
            var result = await controller.Edit(notExistingId, GetNewIngredientModel());

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var action = viewResult.ViewName;
            action.Should().Be("NotFound");
        }
        [Fact]
        public async Task PostEdit_WhenModelStateIsNotValid_ReturnsEditViewWithViewBags()
        {
            // Arrange
            _ingredientsServiceMock.Setup(c => c.GetNewIngredientDropdownsWM()).ReturnsAsync(GetDropdowns());
            _ingredientsServiceMock.Setup(c => c.GetVmByIdAsync(It.IsAny<int>())).ReturnsAsync(GetNewIngredientModel());
            var controller = new IngredientsController(_ingredientsServiceMock.Object);
            controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await controller.Edit(GetNewIngredientModel().Id, GetNewIngredientModel());

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var viewDataCount = viewResult.ViewData.Count;
            viewDataCount.Should().Be(2);
            var action = viewResult.ViewName;
            action.Should().Be("Edit");
        }
        [Fact]
        public async Task PostEdit_WhenModelIsValid_ReturnsRedirectToAction()
        {
            // Arrange
            _ingredientsServiceMock.Setup(c => c.UpdateAsync(GetNewIngredientModel()));
            var controller = new IngredientsController(_ingredientsServiceMock.Object);

            // Act
            var result = await controller.Edit(GetNewIngredientModel().Id, GetNewIngredientModel());

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            viewResult.ActionName.Should().Be("Index");
        }
        [Fact]
        public async Task GetDelete_WhenIngredientNotExists_ReturnsNotFoundView()
        {
            // Arrange
            _ingredientsServiceMock.Setup(c => c.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(null as Ingredient);
            var controller = new IngredientsController(_ingredientsServiceMock.Object);

            // Act
            var result = await controller.Delete(It.IsAny<int>());

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var action = viewResult.ViewName;
            action.Should().Be("NotFound");
        }
        [Fact]
        public async Task GetDelete_WhenIngredientExists_ReturnsDeleteView()
        {
            // Arrange
            _ingredientsServiceMock.Setup(c => c.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Ingredient());
            var controller = new IngredientsController(_ingredientsServiceMock.Object);

            // Act
            var result = await controller.Delete(It.IsAny<int>());

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            viewResult.ViewName.Should().Be("Delete");
        }
        [Fact]
        public async Task PostDelete_WhenIngredientNotExists_ReturnsNotFoundView()
        {
            // Arrange
            _ingredientsServiceMock.Setup(c => c.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(null as Ingredient);
            var controller = new IngredientsController(_ingredientsServiceMock.Object);

            // Act
            var result = await controller.DeleteConfirmed(It.IsAny<int>());

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var action = viewResult.ViewName;
            action.Should().Be("NotFound");
        }
        [Fact]
        public async Task PostDelete_WhenIngredientExists_ReturnsRedirectToActionIndex()
        {
            // Arrange
            _ingredientsServiceMock.Setup(c => c.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Ingredient());
            var controller = new IngredientsController(_ingredientsServiceMock.Object);

            // Act
            var result = await controller.DeleteConfirmed(It.IsAny<int>());

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            viewResult.ActionName.Should().Be("Index");
        }

        private static NewIngredientVM GetNewIngredientModel()
        {
            var ingredient = new NewIngredientVM()
            {
                Name = "test",
                Description = "test",
                IngredientTypeId = 1,
                UnitOfMeasureId = 1
            };
            return ingredient;
        }

        private static List<Ingredient> GetIngredients()
        {
            var ingredients = new List<Ingredient>()
            {
                new Ingredient()
                {
                    Id = 1,
                    Name = "Papryka",
                    Description = "Rodzaj roślin należących do rodziny psiankowatych.",
                    IngredientTypeId = 3,
                    UnitOfMeasureId = 5
                },
                new Ingredient()
                {
                    Id = 2,
                    Name = "Marchewka",
                    Description = "Podgatunek marchwi zwyczajnej z licznymi odmianami jadalnymi i pastewnymi.",
                    IngredientTypeId = 3,
                    UnitOfMeasureId = 5
                }
            };
            return ingredients;
        }
        private static NewIngredientDropdownsWM GetDropdowns()
        {
            var dropdowns = new NewIngredientDropdownsWM()
            {
                IngredientTypes = new List<IngredientType>()
                {
                    new IngredientType()
                    {
                        Name = "Przyprawa"
                    },
                    new IngredientType()
                    {
                        Name = "Owoc"
                    },
                },
                UnitOfMeasures = new List<UnitOfMeasure>()
                {
                    new UnitOfMeasure()
                    {
                        Name = "Kilogram",
                        Shortcut = "Kg"
                    },
                    new UnitOfMeasure()
                    {
                        Name = "Gram",
                        Shortcut = "g"
                    }
                }
            };
            return dropdowns;
        }
    }
}
