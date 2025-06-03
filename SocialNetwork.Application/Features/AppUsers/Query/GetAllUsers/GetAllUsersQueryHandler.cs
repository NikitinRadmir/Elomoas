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
using System.Threading;

namespace Elomoas.Application.Features.AppUsers.Query.GetAllUsers
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<GetAllUsersDto>>
    {
        private readonly IAppUserRepository _userRepository;
        private readonly IFriendshipRepository _friendshipRepository;
        private readonly ICurrentUserService _currentUserService;

        public GetAllUsersQueryHandler(
            IAppUserRepository userRepository,
            IFriendshipRepository friendshipRepository,
            ICurrentUserService currentUserService)
        {
            _userRepository = userRepository;
            _friendshipRepository = friendshipRepository;
            _currentUserService = currentUserService;
        }

        public async Task<IEnumerable<GetAllUsersDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAllUsersAsync();
            var currentUserId = _currentUserService.UserId;
            if (!currentUserId.HasValue)
                return Enumerable.Empty<GetAllUsersDto>();

            var result = new List<GetAllUsersDto>();
            foreach (var user in users)
            {
                if (user.Id == currentUserId.Value)
                    continue;

                var friendshipStatus = await _friendshipRepository.GetFriendshipStatusAsync(currentUserId.Value, user.Id);

                result.Add(new GetAllUsersDto
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    Description = user.Description,
                    ProfileImage = user.ProfileImage,
                    FriendshipStatus = friendshipStatus
                });
            }

            return result;
        }
    }
}
