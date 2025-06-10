using Microsoft.AspNetCore.Mvc;

namespace SocialNetwork.Controllers;

public class ErrorController : Controller
{
    [Route("Error/NotFound")]
    public new IActionResult NotFound()
    {
        return View();
    }
} 