using Poetry.ComponentSupport;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;

namespace Poetry.EmbeddedResourceSupport
{
    public class EmbeddedResourceProvider : IEmbeddedResourceProvider
    {
        IEmbeddedResourcePathMatcher EmbeddedResourcePathMatcher { get; }
        IDictionary<string, ComponentDescriptor> Components { get; }
        IDictionary<string, IEnumerable<EmbeddedResource>> EmbeddedResources { get; }

        public EmbeddedResourceProvider(IComponentProvider componentProvider, IEmbeddedResourceCreator embeddedResourceCreator, IEmbeddedResourcePathMatcher embeddedResourcePathMatcher)
        {
            EmbeddedResourcePathMatcher = embeddedResourcePathMatcher;

            Components = componentProvider.GetAll().ToDictionary(c => c.Assembly.Assembly.GetName().Name, c => c);
            EmbeddedResources = componentProvider.GetAll().ToDictionary(c => c.Id, c => (IEnumerable<EmbeddedResource>)embeddedResourceCreator.Create(c).ToList().AsReadOnly());
        }

        public EmbeddedResource Get(string componentId, string path)
        {
            return EmbeddedResources[componentId].FirstOrDefault(e => EmbeddedResourcePathMatcher.Matches(e, path));
        }

        public Stream Open(EmbeddedResource embeddedResource)
        {
            return Components[embeddedResource.AssemblyName].Assembly.Assembly.GetManifestResourceStream(embeddedResource.Name);
        }
    }
}
