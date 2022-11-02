using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ZeroWaste.Data.Static;
using ZeroWaste.Data.ViewModels.Login;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Handlers.Account;

public class AccountHandler : IAccountHandler
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AccountHandler(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }
    public async Task<ApplicationUser> GetByEmailAsync(string emailAddress)
    {
        return await _userManager
            .FindByEmailAsync(emailAddress);
    }
    public async Task<SignInResult> SignIn(ApplicationUser applicationUser, string password)
    {
        return await _signInManager
            .PasswordSignInAsync(applicationUser, password, false, false);
    }
    public async Task<bool> IsPasswordCorrectAsync(ApplicationUser applicationUser, string password)
    {
        return await _userManager.CheckPasswordAsync(applicationUser, password);
    }
    public async Task CreateAsync(RegisterVm registerVm)
    {
        var newUser = new ApplicationUser()
        {
            FullName = registerVm.FullName,
            Email = registerVm.EmailAddress,
            UserName = registerVm.EmailAddress,
        };
        var newUserResponse = await _userManager.CreateAsync(newUser, registerVm.Password);
        if (newUserResponse.Succeeded)
        {
            await _userManager.AddToRoleAsync(newUser, UserRoles.User);
        }
    }

    public async Task Signout()
    {
        await _signInManager.SignOutAsync();
    }

    public async Task<IdentityResult> ChangePasswordAsync(ApplicationUser applicationUser, string oldPassword, string newPassword)
    {
        var result = await _userManager.ChangePasswordAsync(applicationUser, oldPassword, newPassword);
        return result;
    }

    public async Task<ApplicationUser> GetByIdAsync(string id)
    {
        return await _userManager.FindByIdAsync(id);
    }
}