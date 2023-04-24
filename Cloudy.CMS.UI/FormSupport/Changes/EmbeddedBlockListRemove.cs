using System.Text.Json;

namespace Cloudy.CMS.UI.FormSupport.Changes
{
    public class EmbeddedBlockListRemove : EntityChange
    {
        public JsonElement Key { get; set; }
        public string Type { get; set; }
    }
}
