namespace Brokers.RabbitMq;

public class RabbitMqOptionsBuilder
{
    private RabbitMqSettings? _settings;
    private string? _selectedQueue;

    public RabbitMqOptionsBuilder UseSettings(RabbitMqSettings settings)
    {
        _settings = settings;
        return this;
    }

    public RabbitMqOptionsBuilder UseQueue(string queueKey)
    {
        _selectedQueue = queueKey;
        return this;
    }

    public RabbitMqOptionsBuilder UseDefaultQueue()
    {
        _selectedQueue = null;
        return this;
    }

    internal RabbitMqOptions Build()
    {
        ValidateDependencies();
        return new RabbitMqOptions
        {
            Settings = _settings!,
            SelectedQueue = _selectedQueue
        };

        void ValidateDependencies()
        {
            if (_settings is null)
                throw new InvalidOperationException("RabbitMq settings are not specified");
        }
    }
}
