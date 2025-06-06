using MediatR;
using System.Collections.Generic;
using Elomoas.Domain.Entities;
using Elomoas.Application.Interfaces.Services;

namespace Elomoas.Application.Features.Friends.Queries.GetAllFriendships;

public record GetAllFriendshipsQuery : IRequest<IEnumerable<Friendship>>;

public class GetAllFriendshipsQueryHandler : IRequestHandler<GetAllFriendshipsQuery, IEnumerable<Friendship>>
{
    private readonly IFriendshipService _friendshipService;

    public GetAllFriendshipsQueryHandler(IFriendshipService friendshipService)
    {
        _friendshipService = friendshipService;
    }

    public async Task<IEnumerable<Friendship>> Handle(GetAllFriendshipsQuery request, CancellationToken cancellationToken)
    {
        return await _friendshipService.GetAllFriendshipsAsync();
    }
} 