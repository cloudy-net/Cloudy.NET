using Cloudy.CMS.DocumentSupport;
using Cloudy.CMS.DocumentSupport.FileSupport;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Tests
{
    public class DocumentPropertyFinderTests
    {
        [Fact]
        public void FindsGlobalFacetCoreInterfaceProperty()
        {
            var value = new object();

            var document = new Document { GlobalFacet = new DocumentFacet { Interfaces = new Dictionary<string, DocumentInterface> { ["MyCoreInterface"] = new DocumentInterface { Properties = new Dictionary<string, object> { ["MyProperty"] = value } } } } };

            var result = new DocumentPropertyFinder().GetFor(document, "GlobalFacet.Interfaces.MyCoreInterface.Properties.MyProperty");

            Assert.Same(value, result);
        }

        [Fact]
        public void DetectsExistingGlobalFacetCoreInterfaceProperty()
        {
            var value = new object();

            var document = new Document { GlobalFacet = new DocumentFacet { Interfaces = new Dictionary<string, DocumentInterface> { ["MyCoreInterface"] = new DocumentInterface { Properties = new Dictionary<string, object> { ["MyProperty"] = value } } } } };

            var result = new DocumentPropertyFinder().Exists(document, "GlobalFacet.Interfaces.MyCoreInterface.Properties.MyProperty");

            Assert.True(result);
        }
    }
}
