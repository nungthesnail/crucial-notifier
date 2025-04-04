using System;
using Avalonia.Controls;
using Crucial.App.Interfaces;
using Crucial.App.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Crucial.App.Implementations;

public static class PageFactory
{
    public static (UserControl, IServiceScope) Create(PageName pageName, IServiceProvider serviceProvider,
        params object?[] args)
    {
        var scopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();
        var scope = scopeFactory.CreateScope();
        
        var pageBuilder = scope.ServiceProvider.GetRequiredKeyedService<IPageBuilder>(pageName);
        pageBuilder.SetArguments(args);
        var result = pageBuilder.Build();
        return (result, scope);
    }
}
