using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;
using ZeroWaste.Data.Handlers.Account;
using ZeroWaste.Data.Static;
using ZeroWaste.Data.ViewModels.Login;
using ZeroWaste.Models;

namespace ZeroWaste.Controllers;

public class AccountController : Controller
{
    private readonly IAccountHandler _accountHandler;
    public AccountController(IAccountHandler accountHandler)
    {
        _accountHandler = accountHandler;
    }
    public IActionResult Login() => View(new LoginVm());
    [HttpPost]
    public async Task<IActionResult> Login(LoginVm loginVm)
    {
        if (!ModelState.IsValid) return View(nameof(Login), loginVm);
        var user = await _accountHandler.GetByEmailAsync(loginVm.EmailAddress);
        if (user != null)
        {
            if (await _accountHandler.IsPasswordCorrectAsync(user, loginVm.Password))
            {
                if (_accountHandler.SignIn(user, loginVm.Password).Result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            TempData["Error"] = "Niepoprawny login lub hasło. Spróbuj jeszcze raz!";
            return View(nameof(Login), loginVm);
        }
        TempData["Error"] = "Niepoprawny login lub hasło. Spróbuj jeszcze raz!";
        return View(nameof(Login),loginVm);
    }
    public IActionResult Register() => View(new RegisterVm());
    [HttpPost]
    public async Task<IActionResult> Create(RegisterVm registerVm)
    {
        if (!ModelState.IsValid) return View(nameof(Register), registerVm);
        var user = await _accountHandler.GetByEmailAsync(registerVm.EmailAddress);
        if (user != null)
        {
            TempData["Error"] = "Użytkownik jest już zarejestrowany";
            return View(nameof(Register), registerVm);
        }
        await _accountHandler.CreateAsync(registerVm);
        return View("RegisterCompleted");
    }
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await _accountHandler.Signout();
        return RedirectToAction("Index", "Home");
    }
    [Authorize]
    public IActionResult ChangePassword()
    {
        return View(new ChangePasswordVM());
    }
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> ChangePassword(ChangePasswordVM changePasswordVM)
    {
        if (!ModelState.IsValid)
        {
            return View(changePasswordVM);
        }

        string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        var user = await _accountHandler.GetByIdAsync(userId);
        if (user is null)
        {
            TempData["Error"] = "Użytkownik nie istnieje";
            return View(changePasswordVM);
        }

        var changePasswordResult = await _accountHandler.ChangePasswordAsync(user, changePasswordVM.OldPassword, changePasswordVM.NewPassword);

        if(Equals(changePasswordResult.Succeeded, false))
        {
            string errors = string.Join(" ", changePasswordResult.Errors.Select(e => e.Code.TranslateErrorCodeToPolish()));
            TempData["Error"] = errors;
            return View(changePasswordVM);
        }

        return RedirectToAction("Index", "Home");
    }
}
