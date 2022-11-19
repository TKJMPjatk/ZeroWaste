using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using ZeroWaste.Data.ViewModels.Login;

namespace ZeroWaste.Tests;

public class RegisterVmTests
{
    [Fact]
    public void RegisterVmValidation_ForEmptyFullName_ShouldBeInvalid()
    {
        //Arrange
        var newObject = new RegisterVm()
        {
            EmailAddress = "email@email.com",
            Password = "123",
            ConfirmPassword = "123"
        };
        var context = new ValidationContext(newObject);
        var results = new List<ValidationResult>();
        //Act
        var isModelStateValid = Validator.TryValidateObject(newObject, context, results, true);
        //Assert
        results.Any(v =>
            v.MemberNames.Contains("FullName")
            && v.ErrorMessage.Contains("Pole imię i nazwisko jest wymagane")).Should().BeTrue();
        isModelStateValid.Should().BeFalse();
    }
    [Fact]
    public void RegisterVmValidation_ForEmptyEmail_ShouldBeInvalid()
    {
        //Arrange
        var newObject = new RegisterVm()
        {
            FullName = "TK",
            Password = "123",
            ConfirmPassword = "123"
        };
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
    public void RegisterVmValidation_ForEmptyPassword_ShouldBeInvalid()
    {
        //Arrange
        var newObject = new RegisterVm()
        {
            EmailAddress = "email@email.com",
            FullName = "TK",
            ConfirmPassword = "123"
        };
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
    public void RegisterVmValidation_ForEmptyConfirmPassword_ShouldBeInvalid()
    {
        //Arrange
        var newObject = new RegisterVm()
        {
            EmailAddress = "email@email.com",
            FullName = "TK",
            Password = "123"
        };
        var context = new ValidationContext(newObject);
        var results = new List<ValidationResult>();
        //Act
        var isModelStateValid = Validator.TryValidateObject(newObject, context, results, true);
        //Assert
        results.Any(v =>
            v.MemberNames.Contains("ConfirmPassword")
            && v.ErrorMessage.Contains("Pole potwierdź hasło jest wymagane")).Should().BeTrue();
        isModelStateValid.Should().BeFalse();
    }    
    [Fact]
    public void RegisterVmValidation_ForDiffrentPasswordAndConfirmPassword_ShouldBeInvalid()
    {
        //Arrange
        var newObject = new RegisterVm()
        {
            EmailAddress = "email@email.com",
            FullName = "TK",
            Password = "123",
            ConfirmPassword = "321"
        };
        var context = new ValidationContext(newObject);
        var results = new List<ValidationResult>();
        //Act
        var isModelStateValid = Validator.TryValidateObject(newObject, context, results, true);
        //Assert
        results.Any(v =>
            v.MemberNames.Contains("ConfirmPassword")
            && v.ErrorMessage.Contains("Hasła nie pasują")).Should().BeTrue();
        isModelStateValid.Should().BeFalse();
    }
    [Fact]
    public void RegisterVmValidation_ForNotCorrectEmail_ShouldBeInvalid()
    {
        //Arrange
        var newObject = new RegisterVm()
        {
            EmailAddress = "email",
            FullName = "TK",
            Password = "123",
            ConfirmPassword = "123"
        };
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
    public void RegisterVmValidation_ForCorrectModel_ShouldValid()
    {
        //Arrange
        var newObject = new RegisterVm()
        {
            FullName = "TKTest",
            EmailAddress = "email@email.com",
            Password = "123",
            ConfirmPassword = "123"
        };
        var context = new ValidationContext(newObject);
        var results = new List<ValidationResult>();
        //Act
        var validateResult = Validator.TryValidateObject(newObject, context, results, true);
        //Assert
        results.Any().Should().BeFalse();
        validateResult.Should().BeTrue();
    }
}