using System.Diagnostics;

namespace Poetry.UI.ScriptSupport
{
    [DebuggerDisplay("{ComponentId}/{Path}")]
    public class ScriptDescriptor
    {
        public string ComponentId { get; }
        public string Path { get; }

        public ScriptDescriptor(string componentId, string path)
        {
            ComponentId = componentId;
            Path = path;
        }
    }
}