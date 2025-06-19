using Elomoas.Domain.Entities;
using MediatR;
using Elomoas.Application.Features.Groups.Query.GetAll;

namespace Elomoas.Application.Features.Groups.Queries;

public record GetGroupByIdQuery(int Id) : IRequest<GroupDto?>; 