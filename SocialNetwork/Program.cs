using Elomoas.Persistence.Contexts;
using Elomoas.Persistence.Extensions;
using Elomoas.Application.Extensions;
using Elomoas.Infrastructure.Extensions;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Elomoas.Infrastructure.Settings;
using Elomoas.mvc.Hubs;
using Microsoft.AspNetCore.Identity;
using Elomoas.Extensions;
using Microsoft.Extensions.Logging;
using Elomoas.Logging;

namespace Elomoas;

public class Program
{
    public static void Main(string[] args)
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

        builder.Services.AddControllersWithViews();
        builder.Services.AddControllers().AddJsonOptions(options =>
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        builder.Services.AddSignalR();



        var app = builder.Build();

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

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Feed}/{id?}");
        app.MapControllerRoute(
             name: "Admin",
             pattern: "Admin/{controller=Dashboard}/{action=Dashboard}/{id?}"
        );

        app.MapHub<FriendshipHub>("/friendshipHub");

        app.Run();
    }   
}
