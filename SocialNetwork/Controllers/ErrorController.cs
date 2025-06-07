using Microsoft.AspNetCore.Mvc;

namespace SocialNetwork.Controllers;

public class ErrorController : Controller
{
    [Route("Error/NotFound")]
    public IActionResult NotFound()
    {
        return View();
    }
} 