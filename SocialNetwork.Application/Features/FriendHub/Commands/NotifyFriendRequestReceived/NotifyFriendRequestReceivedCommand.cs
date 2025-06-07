using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Elomoas.Application.Features.FriendHub.Commands.NotifyFriendRequestReceived;

public record NotifyFriendRequestReceivedCommand(string TargetUserId, IdentityUser Sender) : IRequest; 