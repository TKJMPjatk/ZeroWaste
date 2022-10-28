using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ZeroWaste.Data;
using ZeroWaste.Data.Handlers.Account;
using ZeroWaste.Data.Static;
using ZeroWaste.Data.ViewModels.Login;
using ZeroWaste.Models;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

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
        if (!ModelState.IsValid) return View(loginVm);
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
            return View(loginVm);
        }
        TempData["Error"] = "Niepoprawny login lub hasło. Spróbuj jeszcze raz!";
        return View(loginVm);
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
    public async Task<IActionResult> Logout()
    {
        await _accountHandler.Signout();
        return RedirectToAction("Index", "Home");
    }
}
