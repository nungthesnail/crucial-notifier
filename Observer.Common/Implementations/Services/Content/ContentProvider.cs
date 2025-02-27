using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Observer.Common.Exceptions;
using Observer.Common.Interfaces.Services;
using Observer.Common.Models;

namespace Observer.Common.Implementations.Services.Content;

public class ContentProvider : IContentProvider
{
    private static readonly HttpClient HttpClient = new();
    private readonly ILogger<ContentProvider> _logger;
    private readonly FurtherContentParser _parser;
    private readonly IHashCalculator _hashCalculator;
    private readonly string _targetUrl;
    
    public ContentProvider(ILogger<ContentProvider> logger, IHashCalculator hashCalculator, IConfiguration config)
    {
        _logger = logger;
        _hashCalculator = hashCalculator;
        _parser = new FurtherContentParser(config);
        _targetUrl = config.GetValue<string>("ObservingUrl") 
                     ?? throw new BadConfigurationException("ObservingUrl isn't specified");
        
        _logger.LogInformation("Content provider created. TargetUrl: {targetUrl}", _targetUrl);
    }
    
    public async Task<WebPageContent> GetContentAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Getting content from target url...");
        
        var pageContent = await GetTargetResourceAsync(stoppingToken);
        _logger.LogInformation("Content received");
        
        var modifiedTimestamp = _parser.ExtractModifiedTimestamp(pageContent);
        _logger.LogInformation("Timestamp of last modification was {action}",
            modifiedTimestamp.HasValue ? $"extracted: {modifiedTimestamp}" : "not extracted");
        
        var hash = await _hashCalculator.CalculateHashAsync(pageContent);
        _logger.LogInformation("Hash of current content calculated: {hash}", hash);
        
        return BuildContentModel();

        WebPageContent BuildContentModel()
            => new()
            {
                Hash = hash,
                LastModified = modifiedTimestamp
            };
    }

    private async Task<string> GetTargetResourceAsync(CancellationToken stoppingToken)
    {
        try
        {
            stoppingToken.ThrowIfCancellationRequested();
            var message = new HttpRequestMessage(HttpMethod.Get, _targetUrl);
            var response = await HttpClient.SendAsync(message, stoppingToken);
            ThrowIfNotSuccess(response);
            
            return await response.Content.ReadAsStringAsync(stoppingToken);
        }
        catch (HttpRequestException)
        {
            _logger.LogError("Http request to target url ({url}) failed", _targetUrl);
            throw;
        }
        
        void ThrowIfNotSuccess(HttpResponseMessage resp)
        {
            if (resp.IsSuccessStatusCode) return;
            
            var code = (int)resp.StatusCode;
            _logger.LogError("Http request to target url ({url}) is not success. Response code: {code}",
                _targetUrl,code);
            throw new HttpFailedRequestException($"Http response code: {code}");
        }
    }
}
