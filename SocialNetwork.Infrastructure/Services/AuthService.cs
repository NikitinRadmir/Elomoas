using Microsoft.AspNetCore.Identity;
using Elomoas.Application.Interfaces.Services;
using Elomoas.Application.Interfaces.Repositories;
using Elomoas.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elomoas.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IAppUserRepository _userRepository;

    public AuthService(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        IAppUserRepository userRepository)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _userRepository = userRepository;
    }

    public async Task<bool> RegisterAsync(string email, string password, string firstName, string lastName)
    {
        var identityUser = new IdentityUser
        {
            UserName = email,
            Email = email
        };

        var identityResult = await _userManager.CreateAsync(identityUser, password);
        if (!identityResult.Succeeded)
            return false;

        var appUser = new AppUser
        {
            IdentityId = identityUser.Id,
            FullName = $"{firstName} {lastName}",
            Email = email,
            Password = password
        };

        await _userRepository.AddAsync(appUser);
        return true;
    }

    public async Task<bool> LoginAsync(string email, string password, bool rememberMe)
    {
        var result = await _signInManager.PasswordSignInAsync(email, password, rememberMe, false);
        return result.Succeeded;
    }

    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }
}
