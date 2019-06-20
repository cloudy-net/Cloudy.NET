using System.Collections.Generic;
using System.IO;

namespace Poetry.EmbeddedResourceSupport
{
    public interface IEmbeddedResourceProvider
    {
        EmbeddedResource Get(string componentId, string path);
        Stream Open(EmbeddedResource embeddedResource);
    }
}