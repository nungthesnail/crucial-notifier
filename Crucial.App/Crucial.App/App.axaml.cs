using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Crucial.App.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Crucial.App
{
	public class App : Application
	{
		public override void Initialize()
		{
			AvaloniaXamlLoader.Load(this);
		}

		public override void OnFrameworkInitializationCompleted()
		{
			var services = PrepareServices();
			var mainWindow = services.GetRequiredService<MainWindow>();
			
			switch (ApplicationLifetime)
			{
				case IClassicDesktopStyleApplicationLifetime desktop:
					desktop.MainWindow = mainWindow;
					break;
				case ISingleViewApplicationLifetime singleViewPlatform:
					singleViewPlatform.MainView = mainWindow;
					break;
			}

			base.OnFrameworkInitializationCompleted();
		}

		private static ServiceProvider PrepareServices()
		{
			var config = LoadConfiguration();
			
			var servicesCollection = new ServiceCollection();
			servicesCollection
				.AddCommonServices(config)
				.AddViewModels()
				.AddViews();
			return servicesCollection.BuildServiceProvider();

			static IConfiguration LoadConfiguration() => new ConfigurationBuilder()
					.AddJsonFile("appsettings.json")
					.Build();
		}
	}
}
