using Elomoas.Persistence.Contexts;
using Elomoas.Persistence.Extensions;
using Elomoas.Application.Extensions;
using Elomoas.Infrastructure.Extensions;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Elomoas.Infrastructure.Settings;
using Elomoas.mvc.Hubs;
using Microsoft.AspNetCore.Identity;

namespace Elomoas;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add DbContext
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        // Add services
        builder.Services.AddApplicationLayer();
        builder.Services.AddInfrastructureLayer();
        builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
        builder.Services.AddPersistenceLayer(builder.Configuration);

        // Configure authentication
        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Auth/Login";
            options.AccessDeniedPath = "/Auth/Login";
        });

        builder.Services.AddControllersWithViews();
        builder.Services.AddControllers().AddJsonOptions(options =>
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        // Add SignalR
        builder.Services.AddSignalR();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
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

        app.UseHttpsRedirection();
        app.UseStaticFiles(new StaticFileOptions
        {
            ServeUnknownFileTypes = true,
            DefaultContentType = "application/octet-stream"
        });

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Feed}/{id?}");
        app.MapControllerRoute(
             name: "Admin",
             pattern: "Admin/{controller=Dashboard}/{action=Dashboard}/{id?}"
        );

        // Map SignalR hub
        app.MapHub<FriendshipHub>("/friendshipHub");

        app.Run();
    }   
}
