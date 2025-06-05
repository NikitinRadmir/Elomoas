using Elomoas.Application.Interfaces.Services;
using Elomoas.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Elomoas.Persistence.Contexts;
using System.Security.Claims;
using Elomoas.Application.Interfaces.Repositories;

namespace Elomoas.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IAppUserRepository _userRepository;

        public CurrentUserService(
            IHttpContextAccessor httpContextAccessor,
            UserManager<IdentityUser> userManager,
            IAppUserRepository userRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _userRepository = userRepository;
        }

        public ClaimsPrincipal User => _httpContextAccessor.HttpContext?.User;

        public string IdentityId => User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        public async Task<AppUser> GetCurrentAppUserAsync()
        {
            var currentIdentityUser = await _userManager.GetUserAsync(User);
            if (currentIdentityUser == null)
                return null;

            var appUsers = await _userRepository.GetAllUsersAsync();
            return appUsers.FirstOrDefault(u => u.IdentityId == currentIdentityUser.Id);
        }

        public int? UserId
        {
            get
            {
                var user = GetCurrentAppUserAsync().GetAwaiter().GetResult();
                return user?.Id;
            }
        }
    }
}
