using MediatR;

namespace Elomoas.Application.Features.Groups.Command.SubscribeToGroup;

public record SubscribeToGroupCommand(int GroupId) : IRequest<bool>;