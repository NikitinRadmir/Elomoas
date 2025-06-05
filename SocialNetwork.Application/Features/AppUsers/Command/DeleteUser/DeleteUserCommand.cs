using MediatR;

namespace SocialNetwork.Application.Features.AppUsers.Command.DeleteUser;

public class DeleteUserCommand : IRequest<bool>
{
    public int Id { get; set; }
} 