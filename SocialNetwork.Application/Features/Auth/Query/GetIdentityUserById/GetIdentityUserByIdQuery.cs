using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Elomoas.Application.Features.Auth.Query.GetIdentityUserById;

public record GetIdentityUserByIdQuery(string UserId) : IRequest<IdentityUser>; 