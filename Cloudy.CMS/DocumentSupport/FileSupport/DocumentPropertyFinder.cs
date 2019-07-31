using System.Linq;

namespace Cloudy.CMS.DocumentSupport.FileSupport
{
    public class DocumentPropertyFinder : IDocumentPropertyFinder
    {
        public bool Exists(Document document, string path)
        {
            var segments = path.Split('.');

            var facetId = segments[0];

            if (facetId != nameof(Document.GlobalFacet))
            {
                return false;
            }

            var facet = document.GlobalFacet;

            if (facet == null)
            {
                return false;
            }

            if (segments[1] != nameof(DocumentFacet.Interfaces))
            {
                return false;
            }

            var coreInterfaceId = segments[2];

            if (!facet.Interfaces.ContainsKey(coreInterfaceId))
            {
                return false;
            }

            var coreInterface = facet.Interfaces[coreInterfaceId];

            if (coreInterface == null)
            {
                return false;
            }

            if (segments[3] != nameof(DocumentInterface.Properties))
            {
                return false;
            }

            var propertyId = segments[4];

            if (!coreInterface.Properties.ContainsKey(propertyId))
            {
                return false;
            }

            var property = coreInterface.Properties[propertyId];

            return true;
        }

        public object GetFor(Document document, string path)
        {
            var segments = path.Split('.');

            var facetId = segments[0];

            if(facetId != nameof(Document.GlobalFacet))
            {
                return null;
            }

            var facet = document.GlobalFacet;

            if(facet == null)
            {
                return null;
            }

            if(segments[1] != nameof(DocumentFacet.Interfaces))
            {
                return null;
            }

            var coreInterfaceId = segments[2];

            if (!facet.Interfaces.ContainsKey(coreInterfaceId))
            {
                return null;
            }

            var coreInterface = facet.Interfaces[coreInterfaceId];

            if(coreInterface == null)
            {
                return null;
            }

            if (segments[3] != nameof(DocumentInterface.Properties))
            {
                return null;
            }

            var propertyId = segments[4];

            if (!coreInterface.Properties.ContainsKey(propertyId))
            {
                return null;
            }

            var property = coreInterface.Properties[propertyId];

            return property;
        }
    }
}