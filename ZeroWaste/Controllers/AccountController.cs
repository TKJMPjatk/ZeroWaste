using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ZeroWaste.Data;
using ZeroWaste.Data.Static;
using ZeroWaste.Data.ViewModels.Login;
using ZeroWaste.Models;

namespace ZeroWaste.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly AppDbContext _context;
    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, AppDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
    }
    public IActionResult Login()
    {
        return View(new LoginVm());
    }
    [HttpPost]
    public async Task<IActionResult> Login(LoginVm loginVm)
    {
        if (!ModelState.IsValid) return View(loginVm);
        var user = await _userManager
            .FindByEmailAsync(loginVm.EmailAddress);
        if (user != null)
        {
            var passwordCheck = await _userManager.CheckPasswordAsync(user, loginVm.Password);
            if (passwordCheck)
            {
                var result = await _signInManager.PasswordSignInAsync(user, loginVm.Password, false, false);
                if (result.Succeeded)
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
    public IActionResult Register()
    {
        return View(new RegisterVm());
    }

    [HttpPost]
    public async Task<IActionResult> Create(RegisterVm registerVm)
    {
        if (!ModelState.IsValid) return View(nameof(Register), registerVm);
        var user = await _userManager
            .FindByEmailAsync(registerVm.EmailAddress);
        if (user != null)
        {
            TempData["Error"] = "Użytkownik jest już zarejestrowany";
            return View(nameof(Register), registerVm);
        }

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

        return View("RegisterCompleted");
    }
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}