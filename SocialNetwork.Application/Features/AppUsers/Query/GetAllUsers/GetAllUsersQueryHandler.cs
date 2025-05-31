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

            //if (currentUser == null)
            //    return users.Select(user => MapToDto(user
            //        //, null, false
            //        ));

            var dtos = new List<AppUserDto>();

            foreach (var user in users)
            {
                if (user.IdentityId == currentUser.Id)
                    continue; // Skip current user

               /* var friendship = await _friendshipRepository.GetFriendshipAsync(currentUser.Id, user.IdentityId);
                var isOutgoingRequest = friendship != null &&
                                      friendship.Status == FriendshipStatus.Pending &&
                                      friendship.UserId == currentUser.Id;*/

                dtos.Add(MapToDto(user
                    //, friendship?.Status, isOutgoingRequest
                    ));
            }

            return dtos ?? new List<AppUserDto>() { };
        }

        private AppUserDto MapToDto(AppUser user
            //, FriendshipStatus? friendshipStatus, bool isOutgoingRequest
            )
        {
            return new AppUserDto
            {
                IdentityId = user.IdentityId,
                Name = user.Name,
                Email = user.Email,
                Img = user.Img,
                Description = user.Description,
                
                //FriendshipStatus = friendshipStatus,
                
                //IsOutgoingRequest = isOutgoingRequest
            };
        }
    }
}
