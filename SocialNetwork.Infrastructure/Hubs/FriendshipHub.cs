using Microsoft.AspNetCore.SignalR;
using Elomoas.Domain.Entities.Enums;
using Elomoas.Application.Interfaces.Repositories;
using System.Threading.Tasks;

namespace Elomoas.Infrastructure.Hubs
{
    public class FriendshipHub : Hub
    {
        private readonly IAppUserRepository _userRepository;

        public FriendshipHub(IAppUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

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
            var sender = await _userRepository.GetByIdentityIdAsync(senderUserId);
            await Clients.Group(targetUserId).SendAsync("ReceiveFriendRequest", new
            {
                SenderId = senderUserId,
                Id = sender.Id,
                SenderName = senderName,
                SenderEmail = sender.Email
            });
        }

        public async Task NotifyFriendRequestAccepted(string targetUserId, string acceptorUserId, string acceptorName)
        {
            var acceptor = await _userRepository.GetByIdentityIdAsync(acceptorUserId);
            await Clients.Group(targetUserId).SendAsync("FriendRequestAccepted", new
            {
                AcceptorId = acceptorUserId,
                Id = acceptor.Id,
                AcceptorName = acceptorName,
                AcceptorEmail = acceptor.Email
            });
        }

        public async Task NotifyFriendRequestRejected(string targetUserId, string rejectorUserId, string rejectorName)
        {
            var rejector = await _userRepository.GetByIdentityIdAsync(rejectorUserId);
            await Clients.Group(targetUserId).SendAsync("FriendRequestRejected", new
            {
                RejectorId = rejectorUserId,
                Id = rejector.Id,
                RejectorName = rejectorName
            });
        }

        public async Task NotifyFriendRemoved(string targetUserId, string removerId, string removerName)
        {
            var remover = await _userRepository.GetByIdentityIdAsync(removerId);
            await Clients.Group(targetUserId).SendAsync("FriendRemoved", new
            {
                RemoverId = removerId,
                Id = remover.Id,
                RemoverName = removerName
            });
        }
    }
} 