using Elomoas.Domain.Entities;
using MediatR;
using System.Collections.Generic;
using Elomoas.Application.Features.Groups.Query.GetSubscriptions;

namespace Elomoas.Application.Features.Groups.Queries;

public record GetAllGroupSubscriptionsQuery : IRequest<IEnumerable<GroupSubscriptionDto>>; 