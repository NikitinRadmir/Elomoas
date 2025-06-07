using System.Threading;
using System.Threading.Tasks;
using Elomoas.Application.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Elomoas.Application.Features.Auth.Query.GetCurrentIdentityUser;

public class GetCurrentIdentityUserQueryHandler : IRequestHandler<GetCurrentIdentityUserQuery, IdentityUser>
{
    private readonly IAuthService _authService;

    public GetCurrentIdentityUserQueryHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<IdentityUser> Handle(GetCurrentIdentityUserQuery request, CancellationToken cancellationToken)
    {
        return await _authService.GetCurrentIdentityUserAsync();
    }
} 