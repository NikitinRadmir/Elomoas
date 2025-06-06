using MediatR;

namespace Elomoas.Application.Features.Settings.Commands;

public record ChangePasswordCommand(string IdentityId, string CurrentPassword, string NewPassword) : IRequest<bool>; 