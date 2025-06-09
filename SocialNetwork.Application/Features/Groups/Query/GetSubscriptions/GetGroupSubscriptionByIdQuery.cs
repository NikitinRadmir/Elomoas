using Elomoas.Domain.Entities;
using MediatR;

namespace Elomoas.Application.Features.Groups.Queries;

public record GetGroupSubscriptionByIdQuery(int Id) : IRequest<GroupSubscription>; 