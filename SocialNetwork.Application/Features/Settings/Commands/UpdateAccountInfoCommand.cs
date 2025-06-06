using MediatR;
using Elomoas.Application.Features.Settings.Dto;

namespace Elomoas.Application.Features.Settings.Commands;

public record UpdateAccountInfoCommand(string IdentityId, AccountInfoDto Model, string? Base64Image, string WebRootPath) : IRequest<bool>; 