using ZeroWaste.Data.ViewModels.Login;

namespace ZeroWaste.Tests.Helpers;

public static class AccountControllerViewModelsProvider
{
    public static RegisterVm GetRegisterVm()
    {
        return new RegisterVm()
        {
            FullName = "Test123", 
            EmailAddress = "email@email.com", 
            Password = "123", 
            ConfirmPassword = "123"
        };
    }

    public static LoginVm GetLoginVm()
    {
        return new LoginVm()
        {
            EmailAddress = "email@email.com",
            Password = "123"
        };
    }
}