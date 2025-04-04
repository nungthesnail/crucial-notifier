using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Crucial.App.Tests;

public abstract class TestsWithConcreteServicesBase
{
    protected IServiceProvider PrepareServices()
    {
        const string dataSourceKey = "ConnectionStrings:Default";
        const string dataSourceValue = "Data Source=unused.db";

        List<KeyValuePair<string, string?>> config
            = [KeyValuePair.Create<string, string?>(dataSourceKey, dataSourceValue)];
        
        return new ServiceCollection()
            .AddCommonServices(
                new ConfigurationBuilder()
                    .AddInMemoryCollection(config)
                    .Build())
            .AddViewModels()
            .AddViews()
            .BuildServiceProvider();
    }
}
