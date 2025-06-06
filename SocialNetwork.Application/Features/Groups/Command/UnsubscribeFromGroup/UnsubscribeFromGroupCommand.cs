using MediatR;

namespace Elomoas.Application.Features.Groups.Commands.UnsubscribeFromGroup;

public record UnsubscribeFromGroupCommand(int GroupId) : IRequest<bool>; 