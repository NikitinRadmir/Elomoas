using Elomoas.Domain.Entities;
using MediatR;
using System;

namespace Elomoas.Application.Features.CourseSubscriptions.Commands;

public record CreateSubscriptionCommand : IRequest<CourseSubscription>
{
    public int UserId { get; init; }
    public int CourseId { get; init; }
    public decimal SubscriptionPrice { get; init; }
    public int DurationInMonths { get; init; }
    public DateTime ExpirationDate { get; init; }
} 