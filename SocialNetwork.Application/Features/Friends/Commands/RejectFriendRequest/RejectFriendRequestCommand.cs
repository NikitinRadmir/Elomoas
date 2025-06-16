using MediatR;

namespace Elomoas.Application.Features.Friends.Commands.RejectFriendRequest
{
    public record RejectFriendRequestCommand : IRequest<bool>
    {
        public string UserId { get; set; }
        public string FriendId { get; set; }
    }
} 