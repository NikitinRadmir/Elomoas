using MediatR;

namespace Elomoas.Application.Features.Friends.Commands.AcceptFriendRequest
{
    public record AcceptFriendRequestCommand : IRequest<bool>
    {
        public string UserId { get; set; }
        public string FriendId { get; set; }

       
    }
} 