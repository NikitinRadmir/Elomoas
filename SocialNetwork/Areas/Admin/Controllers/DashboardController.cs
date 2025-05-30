﻿using Elomoas.Controllers;
using Elomoas.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Elomoas.mvc.Areas.Admin.Controllers;

[Area("Admin")] // Указываем, что этот контроллер принадлежит области Admin
public class DashboardController : Controller
{
    private readonly ILogger<DashboardController> _logger;
    private readonly IMediator _mediator;

    public DashboardController(ILogger<DashboardController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }
    [Route("/Admin")]
    public async Task<IActionResult> Dashboard()
    {

        return View();
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

