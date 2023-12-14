using Microsoft.AspNetCore.Mvc;

namespace Pustok0.wwwroot.assets
{
    public class Product : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
