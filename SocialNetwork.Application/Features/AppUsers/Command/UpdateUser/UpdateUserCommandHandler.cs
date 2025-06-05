using Elomoas.Application.Interfaces.Services;
using Elomoas.Domain.Entities;
using MediatR;

namespace SocialNetwork.Application.Features.AppUsers.Command.UpdateUser;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, bool>
{
    private readonly IUserService _userService;

    public UpdateUserCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new AppUser
        {
            Id = request.Id,
            Name = request.Name,
            Email = request.Email,
            Description = request.Description,
            Img = request.Img
        };

        return await _userService.UpdateUserAsync(user);
    }
} 