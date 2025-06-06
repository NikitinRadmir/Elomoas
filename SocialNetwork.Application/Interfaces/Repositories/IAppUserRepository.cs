using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elomoas.Domain.Entities;

namespace Elomoas.Application.Interfaces.Repositories
{
    public interface IAppUserRepository
    {
        Task<IEnumerable<AppUser>> GetAllUsersAsync();
        Task<AppUser> GetUserByIdAsync(int id);
        Task<AppUser> GetUserByIdWithIdentityAsync(int id);
        Task<AppUser> GetCurrentUserAsync();
        Task<IEnumerable<AppUser>> GetUsersByIdentityIdsAsync(IEnumerable<string> identityIds);
    }
}
