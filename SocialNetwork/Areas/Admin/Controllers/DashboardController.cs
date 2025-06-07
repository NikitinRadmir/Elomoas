using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace SocialNetwork.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "ManagerPolicy")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

