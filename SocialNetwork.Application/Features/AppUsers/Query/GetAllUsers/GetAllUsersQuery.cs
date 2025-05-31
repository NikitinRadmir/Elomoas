using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Elomoas.Application.Features.AppUsers.Query;

namespace Elomoas.Application.Features.AppUsers.Query.GetAllUsers
{
    public record GetAllUsersQuery() : IRequest<IEnumerable<AppUserDto>>;
}
