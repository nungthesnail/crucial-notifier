using Crucial.App.Implementations.PageArgumentsValidators;
using Crucial.App.Implementations.PageBuilders;
using Crucial.App.Interfaces;
using Crucial.App.Models;
using Crucial.App.ViewModels;
using Crucial.App.Views;
using Crucial.Core.Implementations;
using Crucial.Core.Interfaces.Dal;
using Crucial.Core.Interfaces.Managers;
using Crucial.EntityFrameworkCore;
using Crucial.Implementations.Dal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Crucial.App;

internal static class Startup
{
    public static IServiceCollection AddCommonServices(this IServiceCollection services, IConfiguration config)
    {
        var dataSource = config.GetConnectionString("Default");
        
        // Primary services
        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(dataSource))
            .AddScoped<ICourseManager, CourseManager>()
            .AddScoped<ILessonManager, LessonManager>()
            .AddScoped<IUnitOfWorkFactory, EfUnitOfWorkFactory>()
            .AddScoped<IUnitOfWork>(static provider => provider.GetRequiredService<IUnitOfWorkFactory>().Create())
            .AddSingleton(config)

            // Secondary services
            .AddSerilog(config)

            // Service services
            .AddKeyedTransient<IPageBuilder, MainPageBuilder>(PageName.Main)
            .AddKeyedTransient<IPageArgumentsValidator, MainPageArgumentValidators>(PageName.Main);
        
        return services;
    }

    private static IServiceCollection AddSerilog(this IServiceCollection services, IConfiguration config)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(config)
            .CreateLogger();
        
        return services.AddSingleton(Log.Logger);
    }

    public static IServiceCollection AddViewModels(this IServiceCollection services)
    {
        services.AddScoped<MainViewModel>();
        return services;
    }
    
    public static IServiceCollection AddViews(this IServiceCollection services)
    {
        services.AddSingleton<MainWindow>(static services => new MainWindow(services)
        {
            // DataContext = services.GetRequiredService<MainViewModel>()
        });
        return services;
    }
}
