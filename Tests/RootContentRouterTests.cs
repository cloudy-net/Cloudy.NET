using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.Routing;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Tests
{
    public class RootContentRouterTests
    {
        [Theory]
        [InlineData("lorem", "lorem", true)]
        [InlineData("lorem", "ipsum", false)]
        [InlineData(null, null, true)]
        [InlineData(null, "ipsum", false)]
        public void RoutesCorrectly(string rootSegment, string pathSegment, bool shouldMatch)
        {
            var root = new MyRoot { UrlSegment = rootSegment };

            var segments = new List<string>();

            if(pathSegment != null)
            {
                segments.Add(pathSegment);
            }

            var result = new RootContentRouter(Mock.Of<IContentSegmentRouter>())
                .Route(root, segments, Enumerable.Empty<ContentTypeDescriptor>(), null);

            if (shouldMatch)
            {
                Assert.Same(root, result);
            }
            else
            {
                Assert.Null(result);
            }
        }

        class MyRoot : IContent, IRoutable
        {
            public string Id { get; set; }
            public string ContentTypeId { get; set; }
            public string UrlSegment { get; set; }
        }
    }
}
