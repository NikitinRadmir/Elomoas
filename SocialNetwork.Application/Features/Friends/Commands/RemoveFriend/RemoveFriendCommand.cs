using MediatR;

namespace Elomoas.Application.Features.Friends.Commands.RemoveFriend
{
    public class RemoveFriendCommand : IRequest<bool>
    {
        public string UserId { get; set; }
        public string FriendId { get; set; }

        public RemoveFriendCommand(string userId, string friendId)
        {
            UserId = userId;
            FriendId = friendId;
        }
    }
} 