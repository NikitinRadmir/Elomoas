using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Elomoas.Application.Features.FriendHub.Commands.NotifyFriendRequestAccepted;

public record NotifyFriendRequestAcceptedCommand(string TargetUserId, IdentityUser Accepter) : IRequest; 