using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.Routing;
using Cloudy.CMS.UI.DataTableSupport.BackendSupport;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Patterns;
using Microsoft.Extensions.Logging;
using Moq;
using NuGet.Frameworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Tests
{
    public class ContentRouteCreatorTests
    {
        [Theory]
        [InlineData("lorem", null, null)]
        [InlineData("lorem/{route:contentroute}", "lorem/{contentroute}", "sit,dol")]
        [InlineData("lorem/{route:contentroute(expandToDol)}", "lorem/{contentroute}", "dol")]
        public void CreatesRoutes(string pattern, string resultingTemplate, string resultingTypes)
        {
            var contentTypeA = new ContentTypeDescriptor("sit", typeof(ContentTypeA), "container");
            var contentTypeB = new ContentTypeDescriptor("dol", typeof(ContentTypeB), "container");

            var contentTypeProvider = Mock.Of<IContentTypeProvider>();
            Mock.Get(contentTypeProvider).Setup(p => p.GetAll()).Returns(new List<ContentTypeDescriptor> { contentTypeA, contentTypeB });

            var contentTypeExpander = Mock.Of<IContentTypeExpander>();
            Mock.Get(contentTypeExpander).Setup(e => e.Expand("expandToDol")).Returns(new List<ContentTypeDescriptor> { contentTypeB });

            var endpoint = new RouteEndpoint(async context => { }, RoutePatternFactory.Parse(pattern), 0, null, null);
            var dataSource = new CompositeEndpointDataSource(new List<EndpointDataSource> { new DefaultEndpointDataSource(endpoint) });

            var results = new ContentRouteCreator(Mock.Of<ILogger<ContentRouteCreator>>(), dataSource, contentTypeProvider, contentTypeExpander).Create();

            if(resultingTemplate == null)
            {
                Assert.Empty(results);
                return;
            }

            Assert.Single(results);

            var result = results.Single();

            Assert.Equal(resultingTemplate, result.Template);
            Assert.Equal(resultingTypes, string.Join(",", result.ContentTypes.Select(t => t.Id)));
        }

        class ContentTypeA : InterfaceA
        {
        }

        class ContentTypeB
        {
        }

        class ContentTypeC
        {
        }

        private interface InterfaceA
        {
        }
    }
}
