using System.Diagnostics;

namespace Cloudy.CMS.UI.ScriptSupport
{
    [DebuggerDisplay("{ComponentId}/{Path}")]
    public class ScriptDescriptor
    {
        public string Path { get; }

        public ScriptDescriptor(string path)
        {
            Path = path;
        }
    }
}