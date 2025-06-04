using MediatR;

namespace Elomoas.Application.Features.Friends.Commands.SendFriendRequest
{
    public class SendFriendRequestCommand : IRequest<bool>
    {
        public string UserId { get; set; }
        public string FriendId { get; set; }

        public SendFriendRequestCommand(string userId, string friendId)
        {
            UserId = userId;
            FriendId = friendId;
        }
    }
} 