using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Elomoas.Application.Features.Friends.Dtos;
using Elomoas.Application.Interfaces.Services;
using Elomoas.Domain.Entities;
using MediatR;
using System.Linq;

namespace Elomoas.Application.Features.Friends.Queries.GetAllFriendships;

public class GetAllFriendshipsQueryHandler : IRequestHandler<GetAllFriendshipsQuery, IEnumerable<FriendshipDto>>
{
    private readonly IFriendshipService _friendshipService;

    public GetAllFriendshipsQueryHandler(IFriendshipService friendshipService)
    {
        _friendshipService = friendshipService;
    }

    public async Task<IEnumerable<FriendshipDto>> Handle(GetAllFriendshipsQuery request, CancellationToken cancellationToken)
    {
        var data = await _friendshipService.GetAllFriendshipsAsync();
        var result = data.Select(x => new FriendshipDto
        {
            Id = x.Id,
            UserId = x.UserId,
            FriendId = x.FriendId,
            Status = x.Status,
        });
        return result;
    }
} 