using Elomoas.Application.Interfaces.Services;
using Elomoas.Domain.Entities;
using Elomoas.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Elomoas.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly ApplicationDbContext _context;
    private readonly IEmailService _emailService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthService(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        IEmailService emailService,
        ApplicationDbContext context,
        IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
        _emailService = emailService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<bool> RegisterAsync(string name, string email, string password)
    {
        var identityUser = new IdentityUser
        {
            UserName = email,
            Email = email,
        };

        var result = await _userManager.CreateAsync(identityUser, password);

        if (!result.Succeeded)
            return false;

        await _signInManager.SignInAsync(identityUser, isPersistent: false);

        var user = new AppUser
        {
            IdentityId = identityUser.Id,
            Name = name,
            Email = email,
            Img = "/images/default-icon.jpg",
            Description = "",
            Password = password 
        };

        await _context.AppUsers.AddAsync(user);
        await _context.SaveChangesAsync();

        var subject = "Регистрация прошла успешно";
        var body = $"<h1>Здравствуйте, {name}!</h1>" +
                   "<p>Вы успешно зарегистрировались на нашем сайте Elomoas.</p>" +
                   "<p>Ваши данные: </p>" +
                   $"<p>Login: {email}</p>" +
                   $"<p>Password: {password}</p>";

        await _emailService.SendEmailAsync(email, subject, body);
        return true;
    }

    public async Task<bool> LoginAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            return false;

        var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
        return result.Succeeded;
    }

    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }

    public async Task<IEnumerable<IdentityUser>> GetAllIdentityUsersAsync()
    {
        return await _userManager.Users.ToListAsync();
    }

    public async Task<IdentityUser> GetCurrentIdentityUserAsync()
    {
        if (_httpContextAccessor.HttpContext?.User == null)
            return null;

        return await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
    }

    public async Task<IdentityUser> GetIdentityUserByIdAsync(string userId)
    {
        return await _userManager.FindByIdAsync(userId);
    }
}
