using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Elomoas.Application.Features.AppUsers.Queries.GetAllUsers;

public record GetAllIdentityUsersQuery : IRequest<IEnumerable<IdentityUser>>;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllIdentityUsersQuery, IEnumerable<IdentityUser>>
{

    private readonly UserManager<IdentityUser> _userManager;

    public GetAllUsersQueryHandler(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IEnumerable<IdentityUser>> Handle(GetAllIdentityUsersQuery request, CancellationToken cancellationToken)
    {
        return await _userManager.Users.ToListAsync();
    }
}