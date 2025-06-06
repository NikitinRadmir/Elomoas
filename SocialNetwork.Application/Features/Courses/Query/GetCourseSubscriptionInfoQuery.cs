using MediatR;
using Elomoas.Application.Features.Courses.Dto;

namespace Elomoas.Application.Features.Courses.Query;

public record GetCourseSubscriptionInfoQuery(int AppUserId, int CourseId) : IRequest<SubscriptionInfoDto?>; 