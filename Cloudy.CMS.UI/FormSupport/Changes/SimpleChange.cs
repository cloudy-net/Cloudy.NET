using System.Text.Json;

namespace Cloudy.CMS.UI.FormSupport.Changes
{
    public class SimpleChange : EntityChange
    {
        public JsonElement Value { get; set; }
    }
}
