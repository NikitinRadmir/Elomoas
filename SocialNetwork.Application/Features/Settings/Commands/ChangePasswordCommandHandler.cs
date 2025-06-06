using MediatR;
using Elomoas.Application.Interfaces.Services;

namespace Elomoas.Application.Features.Settings.Commands;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, bool>
{
    private readonly ISettingsService _settingsService;
    public ChangePasswordCommandHandler(ISettingsService settingsService)
    {
        _settingsService = settingsService;
    }
    public async Task<bool> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        return await _settingsService.ChangePasswordAsync(request.IdentityId, request.CurrentPassword, request.NewPassword);
    }
} 