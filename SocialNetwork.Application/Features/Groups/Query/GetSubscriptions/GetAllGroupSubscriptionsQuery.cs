using Elomoas.Domain.Entities;
using MediatR;
using System.Collections.Generic;

namespace Elomoas.Application.Features.Groups.Queries;

public record GetAllGroupSubscriptionsQuery : IRequest<IEnumerable<GroupSubscription>>; 