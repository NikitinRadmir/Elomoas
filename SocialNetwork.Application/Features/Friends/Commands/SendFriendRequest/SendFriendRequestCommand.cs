using MediatR;

namespace Elomoas.Application.Features.Friends.Commands.SendFriendRequest
{
    public record SendFriendRequestCommand : IRequest<bool>
    {
        public string UserId { get; set; }
        public string FriendId { get; set; }
    }
} 