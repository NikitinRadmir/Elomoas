using Elomoas.Application.Features.Groups.Query.GetAll;
using Elomoas.Domain.Entities;
using MediatR;
using System.Collections.Generic;

namespace Elomoas.Application.Features.Groups.Queries;

public record GetAllGroupsQuery : IRequest<IEnumerable<GroupDto>>; 