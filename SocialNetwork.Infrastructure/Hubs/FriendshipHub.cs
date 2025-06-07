using Microsoft.AspNetCore.SignalR;
using Elomoas.Domain.Entities.Enums;

namespace Elomoas.Infrastructure.Hubs
{
    public class FriendshipHub : Hub
    {
        public async Task AddToGroup(string userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);
        }

        public async Task RemoveFromGroup(string userId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
        }

        public async Task NotifyFriendRequest(string targetUserId, string senderUserId, string senderName)
        {
            await Clients.Group(targetUserId).SendAsync("ReceiveFriendRequest", new
            {
                SenderId = senderUserId,
                SenderName = senderName
            });
        }

        public async Task NotifyFriendRequestAccepted(string targetUserId, string acceptorUserId, string acceptorName)
        {
            await Clients.Group(targetUserId).SendAsync("FriendRequestAccepted", new
            {
                AcceptorId = acceptorUserId,
                AcceptorName = acceptorName
            });
        }

        public async Task NotifyFriendRequestRejected(string targetUserId, string rejectorUserId)
        {
            await Clients.Group(targetUserId).SendAsync("FriendRequestRejected", new
            {
                RejectorId = rejectorUserId
            });
        }

        public async Task NotifyFriendRemoved(string targetUserId, string removerId)
        {
            await Clients.Group(targetUserId).SendAsync("FriendRemoved", new
            {
                RemoverId = removerId
            });
        }
    }
} 