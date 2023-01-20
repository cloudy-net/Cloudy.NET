using System.Text.Json.Serialization;

namespace Cloudy.CMS.UI.List
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ListingColumnWidth
    {
        Default,
        Fill,
        Equal,
    }
}
