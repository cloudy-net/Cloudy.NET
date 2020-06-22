using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Tests
{
    public class ContentRouteDescriptorTests
    {
        [Theory]
        [InlineData("lorem/{contentroute}", "ipsum", "lorem/ipsum")]
        public void Apply(string template, string contentRouteSegment, string expectedResult)
        {
            Assert.Equal(expectedResult, new ContentRouteDescriptor(template, Enumerable.Empty<ContentTypeDescriptor>()).Apply(contentRouteSegment));
        }
    }
}
