using MediatR;

namespace SocialNetwork.Application.Features.AppUsers.Command.UpdateUserPassword;

public class UpdateUserPasswordCommand : IRequest<bool>
{
    public int AppUserId { get; set; }
    public string NewPassword { get; set; }
} 