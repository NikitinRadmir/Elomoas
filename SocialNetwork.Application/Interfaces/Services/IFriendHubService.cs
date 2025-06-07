using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Elomoas.Application.Interfaces.Services;

public interface IFriendHubService
{
    Task NotifyFriendRequestReceived(string targetUserId, IdentityUser sender);
    Task NotifyFriendRequestAccepted(string targetUserId, IdentityUser accepter);
    Task NotifyFriendRequestRejected(string targetUserId, IdentityUser rejecter);
    Task NotifyFriendRemoved(string targetUserId, IdentityUser remover);
    Task NotifyFriendRemovedAsync(string friendId, string removerId, string removerName);
} 