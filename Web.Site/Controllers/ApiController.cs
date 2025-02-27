using Microsoft.AspNetCore.Mvc;
using Web.Site.Models.Api;

namespace Web.Site.Controllers;

[ApiController]
[Route("/api/[controller]/[action]")]
public class ApiController : ControllerBase
{
    [HttpGet]
    public IActionResult IsUserSubscribed()
    {
        var result = new BoolResponseModel
        {
            Result = false
        };
        return new JsonResult(result);
    }
}

