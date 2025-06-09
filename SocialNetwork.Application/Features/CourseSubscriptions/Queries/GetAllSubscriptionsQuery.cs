using Elomoas.Domain.Entities;
using MediatR;
using System.Collections.Generic;

namespace Elomoas.Application.Features.CourseSubscriptions.Queries;

public record GetAllSubscriptionsQuery : IRequest<IEnumerable<CourseSubscription>>; 