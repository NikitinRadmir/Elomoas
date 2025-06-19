using Elomoas.Domain.Entities;
using MediatR;
using Elomoas.Application.Features.Groups.Query.GetSubscriptions;

namespace Elomoas.Application.Features.Groups.Queries;

public record GetGroupSubscriptionByIdQuery(int Id) : IRequest<GroupSubscriptionDto>; 