using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elomoas.Application.Interfaces.Services;

public interface IAuthService
{
    Task<bool> RegisterAsync(string name, string email, string password);

    Task<bool> LoginAsync(string email, string password);

    Task LogoutAsync();
    Task<IEnumerable<IdentityUser>> GetAllIdentityUsersAsync();
}
