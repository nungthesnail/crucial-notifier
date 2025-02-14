using System.Globalization;
using System.Text.RegularExpressions;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using Microsoft.Extensions.Configuration;

namespace Observer.Common.Implementations.Services.Content;

public partial class FurtherContentParser
{
    private readonly HtmlParserSettings _settings = new();
    
    public FurtherContentParser(IConfiguration config)
    {
        const string settingsSectionName = "HtmlParsing";
        
        config.GetSection(settingsSectionName).Bind(_settings,
            static options => options.ErrorOnUnknownConfiguration = true);
    }
    
    public DateTimeOffset? ExtractModifiedTimestamp(Stream htmlContent)
    {
        var parser = new HtmlParser().ParseDocument(htmlContent);
        var query = $".{_settings.InterestingClass}";
        var element = parser.QuerySelector(query);
        return element is not null ? ExtractTimestampFromText(element.Text()) : null;
    }

    private DateTimeOffset? ExtractTimestampFromText(string text)
    {
        var date = ExtractDate();
        var time = ExtractTime();
        if (!date.HasValue || !time.HasValue)
            return null;
        
        return date.Value.AddHours(time.Value.Hour).AddMinutes(time.Value.Minute); // Date: dd.MM.YYYY + Time: HH + mm

        DateTimeOffset? ExtractDate()
        {
            try
            {
                var regex = DateRegex();
                var match = regex.Match(text);
                if (match.Success)
                    return DateTimeOffset.Parse(
                        match.Value,
                        CultureInfo.GetCultureInfoByIetfLanguageTag(_settings.CultureCode));
                return null;
            }
            catch (FormatException)
            {
                return null;
            }
        }

        DateTimeOffset? ExtractTime()
        {
            try
            {
                var regex = TimeRegex();
                var match = regex.Match(text);
                if (match.Success)
                    return DateTimeOffset.Parse(
                        match.Value, 
                        CultureInfo.GetCultureInfoByIetfLanguageTag(_settings.CultureCode));
                return null;
            }
            catch (FormatException)
            {
                return null;
            }
        }
    }

    [GeneratedRegex(@"\b\d{2}\.\d{2}\.\d{4}\b")]
    private static partial Regex DateRegex();
    [GeneratedRegex(@"\b(2[0-3]|[01]?[0-9]):([0-5]?[0-9])\b")]
    private static partial Regex TimeRegex();
}
