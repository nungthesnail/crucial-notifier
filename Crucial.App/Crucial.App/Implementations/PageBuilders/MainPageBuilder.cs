using System;
using Avalonia.Controls;
using Crucial.App.Implementations.Abstract;
using Crucial.App.Interfaces;
using Crucial.App.Models;
using Crucial.App.ViewModels;
using Crucial.App.Views;
using Microsoft.Extensions.DependencyInjection;

namespace Crucial.App.Implementations.PageBuilders;

public class MainPageBuilder : BasePageBuilder
{
    private const PageName ThisPage = PageName.Main;
    private readonly IServiceProvider _serviceProvider;
    
    public MainPageBuilder(IServiceProvider serviceProvider)
        : base(serviceProvider.GetRequiredKeyedService<IPageArgumentsValidator>(ThisPage))
    {
        _serviceProvider = serviceProvider;
    }

    protected override UserControl BuildWithArguments(object?[] _)
        => new MainView
        {
            DataContext = _serviceProvider.GetRequiredService<MainViewModel>()
        };
}
