using Elomoas.Domain.Entities;

namespace Elomoas.Application.Interfaces.Services;

public interface IUserService
{
    Task<bool> DeleteUserAsync(int id);
    Task<bool> UpdateUserAsync(AppUser user);
} 