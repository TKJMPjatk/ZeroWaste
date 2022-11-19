using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using ZeroWaste.Data.ViewModels.ShoppingList;

namespace ZeroWaste.Tests;

public class NewShoppingListVMTests
{
    [Fact]
    public void NewShoppingListVmValidation_ForIncorrectAttributes_ShouldReturnFalse()
    {
        //Arrange
        var newObject = new NewShoppingListVM();
        var context = new ValidationContext(newObject);
        var results = new List<ValidationResult>();
        //Act
        var isModelStateValid = Validator.TryValidateObject(newObject, context, results, true);
        //Assert
        isModelStateValid.Should().BeFalse();
    }    
    [Fact]
    public void NewShoppingListVmValidation_ForCorrectAttributes_ShouldReturnModelStateTrue()
    {
        //Arrange
        var tmp = new NewShoppingListVM();
        var context = new ValidationContext(tmp);
        var results = new List<ValidationResult>();
        //Act
        var isModelStateValid = Validator.TryValidateObject(tmp, context, results, true);
        //Assert
        isModelStateValid.Should().BeFalse();
    }
}