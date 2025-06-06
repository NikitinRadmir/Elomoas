using System.Threading.Tasks;
using Elomoas.Application.Features.Settings.Dto;

namespace Elomoas.Application.Interfaces.Services;

public interface ISettingsService
{
    Task<AccountInfoDto?> GetAccountInfoAsync(string identityId);
    Task<bool> UpdateAccountInfoAsync(string identityId, AccountInfoDto model, string? base64Image, string webRootPath);
    Task<bool> ChangePasswordAsync(string identityId, string currentPassword, string newPassword);
} 