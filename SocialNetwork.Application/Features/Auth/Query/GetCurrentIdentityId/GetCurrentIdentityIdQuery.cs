using MediatR;

namespace Elomoas.Application.Features.Auth.Query.GetCurrentIdentityId;

public record GetCurrentIdentityIdQuery() : IRequest<string>; 