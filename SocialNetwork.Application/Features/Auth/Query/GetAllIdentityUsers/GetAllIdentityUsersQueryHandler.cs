using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Elomoas.Application.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Elomoas.Application.Features.Auth.Query.GetAllIdentityUsers;

public class GetAllIdentityUsersQueryHandler : IRequestHandler<GetAllIdentityUsersQuery, IEnumerable<IdentityUser>>
{
    private readonly IAuthService _authService;

    public GetAllIdentityUsersQueryHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<IEnumerable<IdentityUser>> Handle(GetAllIdentityUsersQuery request, CancellationToken cancellationToken)
    {
        return await _authService.GetAllIdentityUsersAsync();
    }
} 