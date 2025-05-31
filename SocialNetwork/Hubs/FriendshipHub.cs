using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Identity;
using Elomoas.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;

namespace Elomoas.mvc.Hubs
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
    }
}