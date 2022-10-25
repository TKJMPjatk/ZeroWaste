using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ZeroWaste.Data;
<<<<<<< HEAD
using ZeroWaste.Data.Handlers.Account;
using ZeroWaste.Data.Static;
using ZeroWaste.Data.ViewModels.Login;
using ZeroWaste.Models;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;
=======
using ZeroWaste.Data.ViewModels.Login;
using ZeroWaste.Models;
>>>>>>> 17d4fe8 (Added Account Controller. Added ViewModels to login and register. Added Views for login and register. Added method in controller to getting View of login and register. Added method to log in)

namespace ZeroWaste.Controllers;

public class AccountController : Controller
{
<<<<<<< HEAD
    private readonly IAccountHandler _accountHandler;
    public AccountController(IAccountHandler accountHandler)
    {
        _accountHandler = accountHandler;
    }
    public IActionResult Login() => View(new LoginVm());
=======
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
>>>>>>> 17d4fe8 (Added Account Controller. Added ViewModels to login and register. Added Views for login and register. Added method in controller to getting View of login and register. Added method to log in)
    [HttpPost]
    public async Task<IActionResult> Login(LoginVm loginVm)
    {
        if (!ModelState.IsValid) return View(loginVm);
<<<<<<< HEAD
        var user = await _accountHandler.GetByEmailAsync(loginVm.EmailAddress);
        if (user != null)
        {
            if (await _accountHandler.IsPasswordCorrectAsync(user, loginVm.Password))
            {
                if (_accountHandler.SignIn(user, loginVm.Password).Result.Succeeded)
=======
        var user = await _userManager
            .FindByEmailAsync(loginVm.EmailAddress);
        if (user != null)
        {
            var passwordCheck = await _userManager.CheckPasswordAsync(user, loginVm.Password);
            if (passwordCheck)
            {
                var result = await _signInManager.PasswordSignInAsync(user, loginVm.Password, false, false);
                if (result.Succeeded)
>>>>>>> 17d4fe8 (Added Account Controller. Added ViewModels to login and register. Added Views for login and register. Added method in controller to getting View of login and register. Added method to log in)
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
<<<<<<< HEAD
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
=======
    public IActionResult Register()
    {
        return View(new RegisterVm());
    }
}
>>>>>>> 17d4fe8 (Added Account Controller. Added ViewModels to login and register. Added Views for login and register. Added method in controller to getting View of login and register. Added method to log in)
