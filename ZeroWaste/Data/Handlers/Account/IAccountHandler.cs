using Microsoft.AspNetCore.Identity;
using ZeroWaste.Data.ViewModels.Login;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Handlers.Account;

public interface IAccountHandler
{
    Task<ApplicationUser> GetByEmailAsync(string emailAddress);
    Task<bool> IsPasswordCorrectAsync(ApplicationUser applicationUser, string password);
    Task<SignInResult> SignIn(ApplicationUser applicationUser, string password);
    Task CreateAsync(RegisterVm registerVm);
    Task Signout();
    Task<IdentityResult> ChangePasswordAsync(ApplicationUser applicationUser, string oldPassword, string newPassword);
    Task<ApplicationUser> GetByIdAsync(string id);
}