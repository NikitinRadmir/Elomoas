using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Elomoas.Application.Features.AppUsers.Queries.GetAllUsers;

public record GetAllUsersQuery : IRequest<IEnumerable<IdentityUser>>; 