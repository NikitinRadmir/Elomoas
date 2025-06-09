using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Elomoas.Application.Interfaces.Services;
using Elomoas.Domain.Entities;
using MediatR;

namespace Elomoas.Application.Features.Friends.Queries.GetAllFriendships;

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