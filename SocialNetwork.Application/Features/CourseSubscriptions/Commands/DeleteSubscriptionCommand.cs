using MediatR;

namespace Elomoas.Application.Features.CourseSubscriptions.Commands;

public record DeleteSubscriptionCommand(int Id) : IRequest<bool>; 