using System;
using Avalonia.Controls;
using Crucial.App.Implementations;
using Crucial.App.Models;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Crucial.App.Views
{
	public partial class MainWindow : Window
	{
		private readonly IServiceProvider _serviceProvider;
		private readonly ILogger _logger;
		private IServiceScope? _pageScope;
		
		public MainWindow(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
			_logger = _serviceProvider.GetRequiredService<ILogger>();
			InitializeComponent();
			ShowPage(PageName.Main, []);
		}

		internal void ShowPage(PageName pageName, object?[] args)
		{
			_pageScope?.Dispose();
			(var page, _pageScope) = PageFactory.Create(pageName, _serviceProvider, args);
			SetUserControl(page);
		}

		private void SetUserControl(UserControl userControl)
		{
			var contentArea = this.FindControl<ContentControl>("ContentArea");
			if (contentArea is null)
			{
				_logger.Fatal("No content area found on MainWindow");
				return;
			}
			
			contentArea.Content = userControl;
		}

		~MainWindow()
		{
			_pageScope?.Dispose();
		}
	}
}
