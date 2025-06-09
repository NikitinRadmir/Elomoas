using Elomoas.Domain.Entities;
using MediatR;

namespace Elomoas.Application.Features.Groups.Queries;

public record GetGroupByIdQuery(int Id) : IRequest<Group>; 