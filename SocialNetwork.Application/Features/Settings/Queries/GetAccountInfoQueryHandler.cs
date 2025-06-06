using MediatR;
using Elomoas.Application.Interfaces.Services;
using Elomoas.Application.Features.Settings.Dto;

namespace Elomoas.Application.Features.Settings.Queries;

public class GetAccountInfoQueryHandler : IRequestHandler<GetAccountInfoQuery, AccountInfoDto?>
{
    private readonly ISettingsService _settingsService;
    public GetAccountInfoQueryHandler(ISettingsService settingsService)
    {
        _settingsService = settingsService;
    }
    public async Task<AccountInfoDto?> Handle(GetAccountInfoQuery request, CancellationToken cancellationToken)
    {
        var vm = await _settingsService.GetAccountInfoAsync(request.IdentityId);
        if (vm == null) return null;
        return new AccountInfoDto
        {
            FirstName = vm.FirstName,
            LastName = vm.LastName,
            Email = vm.Email,
            Description = vm.Description,
            Img = vm.Img
        };
    }
} 