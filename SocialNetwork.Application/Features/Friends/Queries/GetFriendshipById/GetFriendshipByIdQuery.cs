using MediatR;
using Elomoas.Domain.Entities;
using Elomoas.Application.Interfaces.Services;

namespace Elomoas.Application.Features.Friends.Queries.GetFriendshipById;

public record GetFriendshipByIdQuery(int Id) : IRequest<Friendship>;

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