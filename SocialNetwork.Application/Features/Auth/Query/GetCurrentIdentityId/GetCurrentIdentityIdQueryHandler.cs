using System.Threading;
using System.Threading.Tasks;
using Elomoas.Application.Interfaces.Services;
using MediatR;

namespace Elomoas.Application.Features.Auth.Query.GetCurrentIdentityId;

public class GetCurrentIdentityIdQueryHandler : IRequestHandler<GetCurrentIdentityIdQuery, string>
{
    private readonly ICurrentUserService _currentUserService;

    public GetCurrentIdentityIdQueryHandler(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }

    public Task<string> Handle(GetCurrentIdentityIdQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_currentUserService.IdentityId);
    }
} 