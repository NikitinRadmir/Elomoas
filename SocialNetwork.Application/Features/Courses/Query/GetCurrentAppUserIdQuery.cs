using MediatR;

namespace Elomoas.Application.Features.Courses.Query;

public record GetCurrentAppUserIdQuery(string IdentityId) : IRequest<int?>; 