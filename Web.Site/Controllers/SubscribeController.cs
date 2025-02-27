using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Site.Controllers;

[Authorize]
public class SubscribeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
