using MediatR;
using Elomoas.Application.Interfaces.Services;

namespace Elomoas.Application.Features.Settings.Commands;

public class UpdateAccountInfoCommandHandler : IRequestHandler<UpdateAccountInfoCommand, bool>
{
    private readonly ISettingsService _settingsService;
    public UpdateAccountInfoCommandHandler(ISettingsService settingsService)
    {
        _settingsService = settingsService;
    }
    public async Task<bool> Handle(UpdateAccountInfoCommand request, CancellationToken cancellationToken)
    {
        return await _settingsService.UpdateAccountInfoAsync(request.IdentityId, request.Model, request.Base64Image, request.WebRootPath);
    }
} 