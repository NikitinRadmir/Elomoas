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
        private int? _cachedUserId;

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

        public string IdentityId => User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        public int? UserId
        {
            get
            {
                if (_cachedUserId.HasValue)
                    return _cachedUserId;

                if (string.IsNullOrEmpty(IdentityId))
                    return null;

                var appUser = _context.AppUsers
                    .FirstOrDefault(u => u.IdentityId == IdentityId);

                _cachedUserId = appUser?.Id;
                return _cachedUserId;
            }
        }

        public async Task<AppUser> GetCurrentAppUserAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null || !httpContext.User.Identity.IsAuthenticated)
            {
                throw new UnauthorizedAccessException("The user is not authorized.");
            }

            var identityUser = await _userManager.GetUserAsync(httpContext.User);
            if (identityUser == null)
            {
                throw new InvalidOperationException("The user was not found.");
            }

            var appUser = await _context.AppUsers
                .FirstOrDefaultAsync(u => u.IdentityId == identityUser.Id);

            if (appUser == null)
            {
                throw new InvalidOperationException("The user profile was not found.");
            }

            return appUser;
        }
    }
}
