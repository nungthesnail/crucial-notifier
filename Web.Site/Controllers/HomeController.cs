﻿using Microsoft.AspNetCore.Mvc;

namespace Web.Site.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
