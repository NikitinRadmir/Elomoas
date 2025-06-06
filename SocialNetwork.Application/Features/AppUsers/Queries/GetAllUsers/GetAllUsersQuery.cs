using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Elomoas.Application.Features.AppUsers.Queries.GetAllUsers;

public record GetAllUsersQuery : IRequest<IEnumerable<IdentityUser>>;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<IdentityUser>>
{
    private readonly UserManager<IdentityUser> _userManager;

    public GetAllUsersQueryHandler(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IEnumerable<IdentityUser>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        return await _userManager.Users.ToListAsync();
    }
} 