using MediatR;

namespace SocialNetwork.Application.Features.AppUsers.Command.UpdateUser;

public class UpdateUserCommand : IRequest<bool>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Description { get; set; }
    public string Img { get; set; }
} 