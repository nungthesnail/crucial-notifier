using System.Text.Json.Serialization;

namespace Web.Site.Models.Api;

public class BoolResponseModel
{
    [JsonPropertyName("result")]
    public bool Result { get; set; }
}
