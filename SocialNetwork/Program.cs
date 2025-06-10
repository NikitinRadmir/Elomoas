using Elomoas.Persistence.Contexts;
using Elomoas.Persistence.Extensions;
using Elomoas.Application.Extensions;
using Elomoas.Infrastructure.Extensions;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Elomoas.Infrastructure.Settings;
using Microsoft.AspNetCore.Identity;
using Elomoas.Extensions;
using Microsoft.Extensions.Logging;
using Elomoas.Logging;
using Elomoas.Infrastructure.Hubs;
using Elomoas.Infrastructure.Identity;
using Elomoas.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;
using System.Globalization;

using Microsoft.Extensions.Options;

namespace Elomoas;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();
        builder.Logging.AddDebug();
        builder.Logging.AddFile("logs/app-{Date}.txt"); 
        builder.Logging.SetMinimumLevel(LogLevel.Information);

        builder.Services.AddApplicationLayer();
        builder.Services.AddInfrastructureLayer();
        builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
        builder.Services.AddPersistenceLayer(builder.Configuration);

        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Auth/Login";
            options.AccessDeniedPath = "/Auth/Login";
        });

        builder.Services.AddLocalization(opt => { opt.ResourcesPath = "Resources"; });
        builder.Services.AddMvc()
            .AddViewLocalization()
            .AddDataAnnotationsLocalization();

        builder.Services.Configure<RequestLocalizationOptions>(options =>
        {
            var cultures = new List<CultureInfo> {
                new CultureInfo("en"),
                new CultureInfo("ru")
            };
            options.DefaultRequestCulture = new RequestCulture("en");
            options.SupportedCultures = cultures;
            options.SupportedUICultures = cultures;
            
            // Configure culture providers order
            options.RequestCultureProviders = new List<IRequestCultureProvider>
            {
                new CookieRequestCultureProvider(),
                new AcceptLanguageHeaderRequestCultureProvider()
            };
        });
        
        builder.Services.AddControllersWithViews();
        builder.Services.AddControllers().AddJsonOptions(options =>
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        builder.Services.AddSignalR();


        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminPolicy", policy =>
                policy.RequireRole("Admin"));

            options.AddPolicy("ManagerPolicy", policy =>
                policy.RequireRole("Manager", "Admin"));

            options.AddPolicy("UserPolicy", policy =>
                policy.RequireRole("User", "Manager", "Admin"));
        });

        builder.Services.Configure<HostOptions>(options =>
        {
            options.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
        });

        builder.Services.AddHostedService<SubscriptionExpirationService>();

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                await ApplicationDbContextSeed.SeedDefaultUserAsync(userManager, roleManager);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred while seeding the database.");
            }
        }

        if (!app.Environment.IsDevelopment())
        {
            app.UseStatusCodePagesWithReExecute("/Error/{0}");
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }
        else
        {
            app.UseStatusCodePagesWithReExecute("/Error/{0}");
            app.UseExceptionHandler("/Error");
        }

        var localizationOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>();
        app.UseRequestLocalization(localizationOptions.Value);


        app.UseGlobalExceptionHandling();
        
        app.UseHttpsRedirection();
        app.UseStaticFiles(new StaticFileOptions
        {
            ServeUnknownFileTypes = true,
            DefaultContentType = "application/octet-stream"
        });

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();
        
        app.UseRequestLogging();
        app.UseUserActivityLogging();

        app.Use(async (context, next) =>
        {
            await next();

            if (context.Response.StatusCode == 401 || context.Response.StatusCode == 403)
            {
                context.Response.Redirect("/Error/NotFound");
            }
        });

        app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Feed}/{id?}");

        app.MapHub<FriendshipHub>("/friendshipHub");
        app.MapHub<ChatHub>("/chatHub");

        await app.RunAsync();
    }   
}
