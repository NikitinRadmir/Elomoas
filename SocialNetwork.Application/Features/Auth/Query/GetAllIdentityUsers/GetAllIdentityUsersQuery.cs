using System.Collections.Generic;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Elomoas.Application.Features.Auth.Query.GetAllIdentityUsers;

public record GetAllIdentityUsersQuery() : IRequest<IEnumerable<IdentityUser>>; 