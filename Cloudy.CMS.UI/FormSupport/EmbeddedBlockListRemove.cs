using System.Text.Json;

namespace Cloudy.CMS.UI.FormSupport
{
    public class EmbeddedBlockListRemove : EntityChange
    {
        public JsonElement Key { get; set; }
        public string Type { get; set; }
    }
}
