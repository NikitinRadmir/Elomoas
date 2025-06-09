using MediatR;

namespace SocialNetwork.Application.Features.AppUsers.Command.DeleteUser;

public record DeleteUserCommand : IRequest<bool>
{
    public int Id { get; set; }
} 