using MediatR;

namespace SocialNetwork.Application.Features.AppUsers.Command.UpdateUserPassword;

public record UpdateUserPasswordCommand : IRequest<bool>
{
    public int AppUserId { get; set; }
    public string NewPassword { get; set; }
} 