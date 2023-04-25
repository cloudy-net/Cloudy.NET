using System.Text.Json;

namespace Cloudy.CMS.UI.FormSupport.Changes
{
    public class EmbeddedBlockListAdd : EntityChange
    {
        public string Key { get; set; }
        public string Type { get; set; }
    }
}
