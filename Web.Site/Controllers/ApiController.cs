using Microsoft.AspNetCore.Mvc;
using Web.Site.Models.Api;
using Web.Site.Services.Exceptions;
using Web.Site.Services.Interfaces;

namespace Web.Site.Controllers;

[ApiController]
[Route("/api/[controller]/[action]")]
public class ApiController : ControllerBase
{
    private readonly ISubscriptitionService _subscriptition;

    public ApiController(ISubscriptitionService subscriptition)
    {
        _subscriptition = subscriptition;
    }
    
    [HttpGet]
    [Route("subscribed")]
    public async Task<IActionResult> IsUserSubscribedAsync()
    {
        try
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                var response = new BoolResponseModel(false);
                return new JsonResult(response);
            }

            var subscribed = await _subscriptition.IsUserSubscribedAsync(User.Identity?.Name);
            var successResponse = new BoolResponseModel(subscribed);
            return new JsonResult(successResponse);
        }
        catch (DataNotFoundException)
        {
            var response = new BoolResponseModel(false);
            return new JsonResult(response);
        }
    }
}
