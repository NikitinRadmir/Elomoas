using System.Threading;
using System.Threading.Tasks;
using Elomoas.Application.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Elomoas.Application.Features.Auth.Query.GetIdentityUserById;

public class GetIdentityUserByIdQueryHandler : IRequestHandler<GetIdentityUserByIdQuery, IdentityUser>
{
    private readonly IAuthService _authService;

    public GetIdentityUserByIdQueryHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<IdentityUser> Handle(GetIdentityUserByIdQuery request, CancellationToken cancellationToken)
    {
        return await _authService.GetIdentityUserByIdAsync(request.UserId);
    }
} 