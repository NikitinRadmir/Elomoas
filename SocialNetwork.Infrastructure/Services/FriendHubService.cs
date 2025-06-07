using Elomoas.Application.Interfaces.Services;
using Elomoas.Infrastructure.Hubs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Elomoas.Application.Interfaces.Repositories;

namespace Elomoas.Infrastructure.Services;

public class FriendHubService : IFriendHubService
{
    private readonly IHubContext<FriendshipHub> _hubContext;
    private readonly IAppUserRepository _userRepository;

    public FriendHubService(
        IHubContext<FriendshipHub> hubContext,
        IAppUserRepository userRepository)
    {
        _hubContext = hubContext;
        _userRepository = userRepository;
    }

    public async Task NotifyFriendRequestReceived(string targetUserId, IdentityUser sender)
    {
        var appUser = await _userRepository.GetByIdentityIdAsync(sender.Id);
        await _hubContext.Clients.Group(targetUserId).SendAsync("ReceiveFriendRequest", 
            new { 
                senderId = sender.Id,
                id = appUser.Id,
                senderName = sender.UserName,
                senderEmail = sender.Email
            });
    }

    public async Task NotifyFriendRequestAccepted(string targetUserId, IdentityUser accepter)
    {
        var appUser = await _userRepository.GetByIdentityIdAsync(accepter.Id);
        await _hubContext.Clients.Group(targetUserId).SendAsync("FriendRequestAccepted", 
            new { 
                accepterId = accepter.Id,
                id = appUser.Id,
                accepterName = accepter.UserName,
                accepterEmail = accepter.Email
            });
    }

    public async Task NotifyFriendRequestRejected(string targetUserId, IdentityUser rejecter)
    {
        var appUser = await _userRepository.GetByIdentityIdAsync(rejecter.Id);
        await _hubContext.Clients.Group(targetUserId).SendAsync("FriendRequestRejected", 
            new { 
                rejecterId = rejecter.Id,
                id = appUser.Id
            });
    }

    public async Task NotifyFriendRemoved(string targetUserId, IdentityUser remover)
    {
        var appUser = await _userRepository.GetByIdentityIdAsync(remover.Id);
        await _hubContext.Clients.Group(targetUserId).SendAsync("FriendRemoved", 
            new { 
                removerId = remover.Id,
                id = appUser.Id
            });
    }

    public async Task NotifyFriendRemovedAsync(string friendId, string removerId, string removerName)
    {
        var appUser = await _userRepository.GetByIdentityIdAsync(removerId);
        await _hubContext.Clients.Group(friendId).SendAsync("FriendRemoved", 
            new { 
                removerId = removerId,
                id = appUser.Id,
                removerName = removerName 
            });
    }
} 