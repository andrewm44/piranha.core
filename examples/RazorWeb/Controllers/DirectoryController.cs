using Microsoft.AspNetCore.Mvc;

namespace NoisyWeb.Controllers
{
    public class DirectoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
