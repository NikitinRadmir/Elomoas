using Elomoas.Persistence.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace SocialNetwork.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AppUsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AppUsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _context.AppUsers
                .Select(u => new
                {
                    u.Id,
                    u.IdentityId,
                    u.Name,
                    u.Email,
                    u.Img,
                    u.Description,
                    u.Password,
                })
                .ToListAsync();

            return View(users);
        }
    }
} 