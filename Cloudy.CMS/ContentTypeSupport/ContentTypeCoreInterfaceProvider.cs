using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloudy.CMS.ContentTypeSupport
{
    public class ContentTypeCoreInterfaceProvider : IContentTypeCoreInterfaceProvider
    {
        IDictionary<string, IEnumerable<CoreInterfaceDescriptor>> CoreInterfacesByContentTypeId { get; } = new Dictionary<string, IEnumerable<CoreInterfaceDescriptor>>();

        public ContentTypeCoreInterfaceProvider(IContentTypeProvider contentTypeProvider, ICoreInterfaceProvider coreInterfaceProvider)
        {
            foreach (var contentType in contentTypeProvider.GetAll())
            {
                var coreInterfaces = contentType.Type.GetInterfaces()
                    .Select(i => coreInterfaceProvider.GetFor(i))
                    .Where(i => i != null);

                CoreInterfacesByContentTypeId[contentType.Id] = coreInterfaces;
            }
        }

        public IEnumerable<CoreInterfaceDescriptor> GetFor(string contentTypeId)
        {
            if (!CoreInterfacesByContentTypeId.ContainsKey(contentTypeId))
            {
                return Enumerable.Empty<CoreInterfaceDescriptor>();
            }

            return CoreInterfacesByContentTypeId[contentTypeId];
        }

    }
}
