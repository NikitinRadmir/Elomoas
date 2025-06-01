using MediatR;
using Elomoas.Application.Interfaces.Services;

namespace Elomoas.Application.Features.AppUsers.Query.GetCurrentUser
{
    public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, AppUserDto>
    {
        private readonly ICurrentUserService _currentUserService;

        public GetCurrentUserQueryHandler(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        public async Task<AppUserDto> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            var appUser = await _currentUserService.GetCurrentAppUserAsync();
            
            return new AppUserDto
            {
                Id = appUser.Id,
                IdentityId = appUser.IdentityId,
                Name = appUser.Name,
                Email = appUser.Email,
                Img = appUser.Img,
                Description = appUser.Description
            };
        }
    }
} 