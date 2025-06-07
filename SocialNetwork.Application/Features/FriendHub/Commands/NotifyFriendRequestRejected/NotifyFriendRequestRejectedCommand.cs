using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Elomoas.Application.Features.FriendHub.Commands.NotifyFriendRequestRejected;

public record NotifyFriendRequestRejectedCommand(string TargetUserId, IdentityUser Rejecter) : IRequest; 