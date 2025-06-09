using Elomoas.Domain.Entities;
using MediatR;
using System;

namespace Elomoas.Application.Features.CourseSubscriptions.Commands;

public record UpdateSubscriptionCommand : IRequest<bool>
{
    public int Id { get; init; }
    public int UserId { get; init; }
    public int CourseId { get; init; }
    public decimal SubscriptionPrice { get; init; }
    public int DurationInMonths { get; init; }
    public DateTime ExpirationDate { get; init; }
} 