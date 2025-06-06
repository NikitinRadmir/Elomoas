using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Elomoas.Application.Features.AppUsers.Query.GetAllIdentityUsers
{
    public record GetAllIdentityUsersQuery : IRequest<IEnumerable<IdentityUser>>;

    public class GetAllIdentityUsersQueryHandler : IRequestHandler<GetAllIdentityUsersQuery, IEnumerable<IdentityUser>>
    {
        private readonly UserManager<IdentityUser> _userManager;

        public GetAllIdentityUsersQueryHandler(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IEnumerable<IdentityUser>> Handle(GetAllIdentityUsersQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(_userManager.Users.ToList());
        }
    }
} 