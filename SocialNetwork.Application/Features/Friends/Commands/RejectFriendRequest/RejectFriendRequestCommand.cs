using MediatR;

namespace Elomoas.Application.Features.Friends.Commands.RejectFriendRequest
{
    public class RejectFriendRequestCommand : IRequest<bool>
    {
        public string UserId { get; set; }
        public string FriendId { get; set; }

        public RejectFriendRequestCommand(string userId, string friendId)
        {
            UserId = userId;
            FriendId = friendId;
        }
    }
} 