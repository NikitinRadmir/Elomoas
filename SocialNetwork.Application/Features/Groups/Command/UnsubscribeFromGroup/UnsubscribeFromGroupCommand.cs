using MediatR;

namespace Elomoas.Application.Features.Groups.Command.UnsubscribeFromGroup;

public record UnsubscribeFromGroupCommand(int GroupId) : IRequest<bool>;