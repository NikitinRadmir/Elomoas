using MediatR;
using Elomoas.Application.Features.Settings.Dto;

namespace Elomoas.Application.Features.Settings.Queries;

public record GetAccountInfoQuery(string IdentityId) : IRequest<AccountInfoDto?>; 