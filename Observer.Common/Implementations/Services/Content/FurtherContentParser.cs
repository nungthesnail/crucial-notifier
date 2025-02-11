using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Microsoft.Extensions.Configuration;
using Observer.Common.Exceptions;

namespace Observer.Common.Implementations.Services.Content;

public class FurtherContentParser
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
        var doc = XDocument.Load(htmlContent);
        var root = doc.Root;
        
        if (root is null)
            throw new ParsingException("Something failed while parsing the HTML content");
        
        var element = root
            .Elements()
            .FirstOrDefault(
                x => x.Attribute(_settings.InterestingAttribute)?.Value == _settings.InterestingAttributeValue);
        
        return element is null ? null : ExtractTimestampFromText(element.Value);
    }

    private static DateTimeOffset? ExtractTimestampFromText(string text)
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
                const string regExpr = @"\b\d{2}\.\d{2}\.\d{4}\b";
                var regex = new Regex(regExpr);
                var match = regex.Match(text);
                if (match.Success)
                    return DateTimeOffset.Parse(match.Value, CultureInfo.InvariantCulture);
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
                const string regExpr = @"\b(2[0-3]|[01]?[0-9]):([0-5]?[0-9])\b";
                var regex = new Regex(regExpr);
                var match = regex.Match(text);
                if (match.Success)
                    return DateTimeOffset.Parse(match.Value, CultureInfo.InvariantCulture);
                return null;
            }
            catch (FormatException)
            {
                return null;
            }
        }
    }
}
