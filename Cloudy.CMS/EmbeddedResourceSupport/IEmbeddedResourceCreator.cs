using System.Collections.Generic;
using Poetry.ComponentSupport;

namespace Poetry.EmbeddedResourceSupport
{
    public interface IEmbeddedResourceCreator
    {
        IEnumerable<EmbeddedResource> Create(ComponentDescriptor c);
    }
}