using System.Text.Json.Serialization;

namespace Cloudy.NET.UI.List
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ListingColumnWidth
    {
        Default,
        Fill,
        Equal,
    }
}
