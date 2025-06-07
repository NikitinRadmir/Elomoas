using MediatR;

namespace SocialNetwork.Application.Features.FriendHub.Commands.NotifyFriendRemoved;

public class NotifyFriendRemovedCommand : IRequest
{
    public string FriendId { get; set; }
    public string RemoverId { get; set; }
    public string RemoverName { get; set; }
} 