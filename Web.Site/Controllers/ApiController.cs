using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Site.Models.Api;
using Web.Site.Services.Exceptions;
using Web.Site.Services.Interfaces;

namespace Web.Site.Controllers;

[ApiController]
public class ApiController : ControllerBase
{
    private readonly ILogger<ApiController> _logger;
    private readonly ISubscriptitionService _subscriptition;

    public ApiController(ILogger<ApiController> logger, ISubscriptitionService subscriptition)
    {
        _logger = logger;
        _subscriptition = subscriptition;
    }
    
    [HttpGet]
    [Route("[controller]/subscribed")]
    public async Task<IActionResult> IsUserSubscribedAsync()
    {
        try
        {
            if (!User.Identity?.IsAuthenticated ?? false)
            {
                var response = new BoolResponseModel(false);
                return new JsonResult(response);
            }

            var subscribed = await _subscriptition.IsUserSubscribedAsync(User.Identity?.Name);
            var successResponse = new BoolResponseModel(subscribed);
            return new JsonResult(successResponse);
        }
        catch (DataNotFoundException exc)
        {
            _logger.LogInformation("Data not found: {msg}", exc.Message);
            var response = new BoolResponseModel(false);
            return new JsonResult(response);
        }
    }

    [HttpPost]
    [Route("[controller]/subscribe")]
    [Authorize]
    public async Task<IActionResult> SubscribeAsync()
    {
        try
        {
            var userName = User.Identity?.Name;
            await _subscriptition.SubscribeUserAsync(userName);
            return Ok();
        }
        catch (DataNotFoundException exc)
        {
            _logger.LogInformation("Data not found: {msg}", exc.Message);
            return new NotFoundResult();
        }
    }

    [HttpGet]
    [Route("[controller]/subscribers")]
    [Authorize]
    public async Task<IActionResult> SubscribersAsync()
    {
        var subscribers = await _subscriptition.GetSubscribersAsync();
        return new JsonResult(subscribers);
    }

    [HttpDelete]
    [Route("[controller]/unsubscribe")]
    [Authorize]
    public async Task<IActionResult> UnsubscribeAsync()
    {
        try
        {
            var userName = User.Identity?.Name;
            await _subscriptition.UnsubscribeUserAsync(userName);
            return Ok();
        }
        catch (DataNotFoundException exc)
        {
            _logger.LogInformation("Data not found: {msg}", exc.Message);
            return new NotFoundResult();
        }
    }
}
