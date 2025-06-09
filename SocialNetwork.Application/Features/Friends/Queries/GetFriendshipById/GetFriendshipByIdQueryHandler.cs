using System.Threading;
using System.Threading.Tasks;
using Elomoas.Application.Interfaces.Services;
using Elomoas.Domain.Entities;
using MediatR;

namespace Elomoas.Application.Features.Friends.Queries.GetFriendshipById;

public class GetFriendshipByIdQueryHandler : IRequestHandler<GetFriendshipByIdQuery, Friendship>
{
    private readonly IFriendshipService _friendshipService;

    public GetFriendshipByIdQueryHandler(IFriendshipService friendshipService)
    {
        _friendshipService = friendshipService;
    }

    public async Task<Friendship> Handle(GetFriendshipByIdQuery request, CancellationToken cancellationToken)
    {
        return await _friendshipService.GetFriendshipByIdAsync(request.Id);
    }
} 