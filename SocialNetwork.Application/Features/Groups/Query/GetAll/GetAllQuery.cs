using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elomoas.Application.Features.Groups.Query.GetAll;

public record GetAllQuery() : IRequest<IEnumerable<GetAllDto>>;