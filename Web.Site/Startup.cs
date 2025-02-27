using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Web.Site.Data;
using Web.Site.Services.Implementations.Subscriptition;
using Web.Site.Services.Interfaces;

namespace Web.Site;

public static class Startup
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        ConfigureWebApp(builder);
        ConfigureCustomServices(builder.Services);
    }

    private static void ConfigureWebApp(WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("Postgres") ??
                            throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>();
        builder.Services.AddControllersWithViews();

        builder.Logging.ClearProviders().AddSerilog(Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.FromLogContext()
            .CreateLogger());
    }

    private static void ConfigureCustomServices(IServiceCollection services)
    {
        services.AddTransient<ISubscriptitionService, SubscriptitionService>();
    }

    public static void ConfigureMiddlewares(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();
        
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=home}/{action=Index}/{id?}");
        app.MapRazorPages();
    }
}
