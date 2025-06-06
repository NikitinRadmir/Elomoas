using MediatR;
using Microsoft.AspNetCore.Identity;
using Elomoas.Application.Interfaces.Repositories;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Elomoas.Application.Features.Settings.Queries.GetAccountInfo
{
    public class GetAccountInfoQueryHandler : IRequestHandler<GetAccountInfoQuery, GetAccountInfoDto>
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IAppUserRepository _userRepository;

        public GetAccountInfoQueryHandler(
            UserManager<IdentityUser> userManager,
            IAppUserRepository userRepository)
        {
            _userManager = userManager;
            _userRepository = userRepository;
        }

        public async Task<GetAccountInfoDto> Handle(GetAccountInfoQuery request, CancellationToken cancellationToken)
        {
            var identityUser = await _userManager.FindByIdAsync(request.IdentityId);
            if (identityUser == null)
                return null;

            var appUser = await _userRepository.GetByIdentityIdAsync(request.IdentityId);
            if (appUser == null)
                return null;

            var nameParts = appUser.Name?.Split(' ') ?? new string[0];
            var dto = new GetAccountInfoDto
            {
                FirstName = nameParts.Length > 0 ? nameParts[0] : "",
                LastName = nameParts.Length > 1 ? string.Join(" ", nameParts.Skip(1)) : "",
                Email = appUser.Email,
                Description = appUser.Description,
                Img = appUser.Img
            };

            return dto;
        }
    }
} 