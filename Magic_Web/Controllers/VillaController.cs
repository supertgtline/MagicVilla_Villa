using Microsoft.AspNetCore.Mvc;

namespace Magic_Web.Controllers;

public class VillaController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}