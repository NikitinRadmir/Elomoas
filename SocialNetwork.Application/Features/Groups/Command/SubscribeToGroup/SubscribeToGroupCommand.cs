using MediatR;

namespace Elomoas.Application.Features.Groups.Commands.SubscribeToGroup;

public record SubscribeToGroupCommand(int GroupId) : IRequest<bool>; 