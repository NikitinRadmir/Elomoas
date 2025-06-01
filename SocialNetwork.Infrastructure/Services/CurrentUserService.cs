using Elomoas.Application.Interfaces.Services;
using Elomoas.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Elomoas.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Elomoas.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public CurrentUserService(
            IHttpContextAccessor httpContextAccessor, 
            UserManager<IdentityUser> userManager,
            ApplicationDbContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _context = context;
        }

        public ClaimsPrincipal User => _httpContextAccessor.HttpContext?.User;

        public string UserId => User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        public async Task<AppUser> GetCurrentAppUserAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null || !httpContext.User.Identity.IsAuthenticated)
            {
                throw new UnauthorizedAccessException("Пользователь не авторизован.");
            }

            var identityUser = await _userManager.GetUserAsync(httpContext.User);
            if (identityUser == null)
            {
                throw new InvalidOperationException("Пользователь не найден.");
            }

            var appUser = await _context.AppUsers
                .FirstOrDefaultAsync(u => u.IdentityId == identityUser.Id);

            if (appUser == null)
            {
                throw new InvalidOperationException("Профиль пользователя не найден.");
            }

            return appUser;
        }
    }
}
