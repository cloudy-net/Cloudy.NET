using System;
using System.Collections.Generic;
using System.Text;

namespace Poetry.EmbeddedResourceSupport
{
    public class EmbeddedResourcePathMatcher : IEmbeddedResourcePathMatcher
    {
        public bool Matches(EmbeddedResource embeddedResource, string path)
        {
            path = path.ToLower();
            
            if(!embeddedResource.Name.StartsWith(embeddedResource.AssemblyName + "."))
            {
                return false;
            }

            var name = embeddedResource.Name.Substring(embeddedResource.AssemblyName.Length + ".".Length).ToLower();

            if(path == name)
            {
                return true;
            }

            if(path.Length != name.Length)
            {
                return false;
            }

            for(var i = 0; i < path.Length; i++)
            {
                var a = path[i];
                var b = name[i];

                if ((a == '-' || a == '_') && (b == '-' || b == '_'))
                {
                    continue;
                }

                if ((a == '.' || a == '/') && (b == '.' || b == '/'))
                {
                    continue;
                }

                if (a == b)
                {
                    continue;
                }

                return false;
            }

            return true;
        }
    }
}
