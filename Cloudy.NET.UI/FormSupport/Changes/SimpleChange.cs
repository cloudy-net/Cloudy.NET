using System.Text.Json;

namespace Cloudy.NET.UI.FormSupport.Changes
{
    public class SimpleChange : EntityChange
    {
        public JsonElement Value { get; set; }
    }
}
