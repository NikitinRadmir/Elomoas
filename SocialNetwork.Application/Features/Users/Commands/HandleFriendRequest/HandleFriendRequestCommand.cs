using MediatR;

namespace Elomoas.Application.Features.Users.Commands.HandleFriendRequest
{
    public class HandleFriendRequestCommand : IRequest<bool>
    {
        public HandleFriendRequestCommand(string userId, string friendId, string action)
        {
            UserId = userId;
            FriendId = friendId;
            Action = action;
        }

        public string UserId { get; private set; }
        public string FriendId { get; private set; }
        public string Action { get; private set; }
    }
} 