using Crucial.App.ViewModels;
using Crucial.Core.Implementations;
using Crucial.Core.Interfaces.Dal;
using Crucial.Core.Interfaces.Managers;
using Crucial.EntityFrameworkCore;
using Crucial.Implementations.Dal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Crucial.App;

internal static class Startup
{
    public static IServiceCollection AddCommonServices(this IServiceCollection services, IConfiguration config)
    {
        var dataSource = config.GetConnectionString("Default");
        
        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(dataSource))
            .AddScoped<ICourseManager, CourseManager>()
            .AddScoped<ILessonManager, LessonManager>()
            .AddScoped<IUnitOfWorkFactory, EfUnitOfWorkFactory>()
            .AddScoped<IUnitOfWork>(static provider => provider.GetRequiredService<IUnitOfWorkFactory>().Create())
            .AddSingleton(config);
        
        return services;
    }

    public static IServiceCollection AddViewModels(this IServiceCollection services)
    {
        services.AddScoped<MainViewModel>();
        return services;
    }
}
