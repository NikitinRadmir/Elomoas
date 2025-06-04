using MediatR;

namespace Elomoas.Application.Features.Friends.Commands.AcceptFriendRequest
{
    public class AcceptFriendRequestCommand : IRequest<bool>
    {
        public string UserId { get; set; }
        public string FriendId { get; set; }

        public AcceptFriendRequestCommand(string userId, string friendId)
        {
            UserId = userId;
            FriendId = friendId;
        }
    }
} 