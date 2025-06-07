using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Elomoas.Application.Features.Auth.Query.GetCurrentIdentityUser;

public record GetCurrentIdentityUserQuery() : IRequest<IdentityUser>; 