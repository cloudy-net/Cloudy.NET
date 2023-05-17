using System.Text.Json;

namespace Cloudy.NET.UI.FormSupport.Changes
{
    public class EmbeddedBlockListRemove : EntityChange
    {
        public string Key { get; set; }
        public string Type { get; set; }
    }
}
