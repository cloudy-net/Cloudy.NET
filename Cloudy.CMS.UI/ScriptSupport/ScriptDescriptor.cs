using System.Diagnostics;

namespace Cloudy.CMS.UI.ScriptSupport
{
    [DebuggerDisplay("{Path}")]
    public class ScriptDescriptor
    {
        public string Path { get; }

        public ScriptDescriptor(string path)
        {
            Path = path;
        }
    }
}