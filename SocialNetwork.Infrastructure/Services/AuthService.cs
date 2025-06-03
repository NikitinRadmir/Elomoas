using Elomoas.Application.Interfaces.Services;
using Elomoas.Domain.Entities;
using Elomoas.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;
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
    private readonly ApplicationDbContext _context;
    private readonly IEmailService _emailService;

    public AuthService(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        IEmailService emailService,
        ApplicationDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
        _emailService = emailService;
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

        // Создаем доменную сущность User
        var user = new AppUser
        {
            IdentityId = identityUser.Id,
            Name = name,
            Email = email,
            Img = "/images/user-12.png",
            Description = "",
            Password = password // Примечание: в реальном приложении не стоит хранить пароль в открытом виде
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
}
