using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_Web.Controllers;

public class HomeController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}