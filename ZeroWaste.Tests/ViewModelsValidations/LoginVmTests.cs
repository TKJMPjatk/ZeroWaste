using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using ZeroWaste.Data.ViewModels.Login;

namespace ZeroWaste.Tests;

public class LoginVmTests
{
    [Fact]
    public void LoginVmValidation_ForEmptyEmail_ShouldReturnFalse()
    {
        //Arrange
        var newObject = new LoginVm {Password = "123"};
        var context = new ValidationContext(newObject);
        var results = new List<ValidationResult>();
        //Act
        var isModelStateValid = Validator.TryValidateObject(newObject, context, results, true);
        //Assert
        results.Any(v =>
            v.MemberNames.Contains("EmailAddress")
            && v.ErrorMessage.Contains("Pole email jest wymagane")).Should().BeTrue();
        isModelStateValid.Should().BeFalse();
    }
    [Fact]
    public void LoginVmValidation_ForEmptyPassword_ShouldReturnFalse()
    {
        //Arrange
        var newObject = new LoginVm {EmailAddress = "email@email@com"};
        var context = new ValidationContext(newObject);
        var results = new List<ValidationResult>();
        //Act
        var isModelStateValid = Validator.TryValidateObject(newObject, context, results, true);
        //Assert
        results.Any(v => 
            v.MemberNames.Contains("Password") 
            && v.ErrorMessage.Contains("Pole hasło jest wymagane")).Should().BeTrue();
        isModelStateValid.Should().BeFalse();
    }

    [Fact]
    public void LoginvmValidation_ForInCorrectEmail_ShouldBeInvalid()
    {
        var newObject = new LoginVm() {EmailAddress = "email", Password = "123"};
        var context = new ValidationContext(newObject);
        var results = new List<ValidationResult>();
        //Act
        var isModelStateValid = Validator.TryValidateObject(newObject, context, results, true);
        //Assert
        results.Any(v => 
            v.MemberNames.Contains("EmailAddress") 
            && v.ErrorMessage.Contains("Adres email ma niepoprawny format")).Should().BeTrue();
        isModelStateValid.Should().BeFalse();
    }
    [Fact]
    public void LoginVmValidation_ForCorrectModel_ShouldReturnTrue()
    {
        //Arrange
        var newObject = new LoginVm {EmailAddress = "email@email.com", Password = "123"};
        var context = new ValidationContext(newObject);
        var results = new List<ValidationResult>();
        //Act
        var isModelStateValid = Validator.TryValidateObject(newObject, context, results, true);
        //Assert
        isModelStateValid.Should().BeTrue();
    }
}