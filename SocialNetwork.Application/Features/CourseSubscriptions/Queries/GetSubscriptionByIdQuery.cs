using Elomoas.Domain.Entities;
using MediatR;

namespace Elomoas.Application.Features.CourseSubscriptions.Queries;

public record GetSubscriptionByIdQuery(int Id) : IRequest<CourseSubscription>; 