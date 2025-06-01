using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elomoas.Application.Features.AppUsers.Query.GetAllUsers;
using MediatR;
using Elomoas.Application.Interfaces.Repositories;
using Elomoas.Application.Features.AppUsers.Query;
using Elomoas.Application.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Elomoas.Domain.Entities;

namespace Elomoas.Application.Features.AppUsers.Query.GetAllUsers
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<AppUserDto>>
    {
        private readonly IAppUserRepository _userRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ICurrentUserService _currentUserService;

        public GetAllUsersQueryHandler(
            IAppUserRepository userRepository,
            UserManager<IdentityUser> userManager,
            ICurrentUserService currentUserService)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _currentUserService = currentUserService;
        }

        public async Task<IEnumerable<AppUserDto>> Handle(GetAllUsersQuery query, CancellationToken ct)
        {
            var users = await _userRepository.GetAllUsersAsync();
            var currentUser = await _userManager.GetUserAsync(_currentUserService.User);

            if (currentUser == null)
                return new List<AppUserDto>();

            var dtos = new List<AppUserDto>();

            foreach (var user in users)
            {
                if (user.IdentityId == currentUser.Id)
                    continue; // Skip current user

                dtos.Add(MapToDto(user));
            }

            return dtos;
        }

        private AppUserDto MapToDto(AppUser user)
        {
            return new AppUserDto
            {
                Id = user.Id,
                IdentityId = user.IdentityId,
                Name = user.Name,
                Email = user.Email,
                Img = user.Img ?? "/images/user-12.png",
                Description = user.Description
            };
        }
    }
}
