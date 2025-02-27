using Microsoft.AspNetCore.Mvc;

namespace Web.Site.Controllers;

public class SubscribeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}