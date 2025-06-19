using MediatR;

namespace SocialNetwork.Application.Features.AppUsers.Command.CreateUser
{
    public class CreateUserCommand : IRequest<bool>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? Description { get; set; }
        public string? Img { get; set; }
    }
} 