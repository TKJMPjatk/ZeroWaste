using Microsoft.AspNetCore.Mvc;

namespace ZeroWaste.Controllers
{
    public class RecipiesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }
    }
}
