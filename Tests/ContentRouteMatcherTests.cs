using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.Routing;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Tests
{
    public class ContentRouteMatcherTests
    {
        [Fact]
        public void MatchesType()
        {
            var contentTypeA = new ContentTypeDescriptor("lorem", typeof(string), "container");
            var contentTypeB = new ContentTypeDescriptor("ipsum", typeof(string), "container");
            
            var routeA = new ContentRouteDescriptor("template", new List<ContentTypeDescriptor> { contentTypeA });
            var routeB = new ContentRouteDescriptor("template", new List<ContentTypeDescriptor> {  });
            var contentRouteProvider = Mock.Of<IContentRouteProvider>();
            Mock.Get(contentRouteProvider).Setup(p => p.GetAll()).Returns(new List<ContentRouteDescriptor> { routeA, routeB });
            
            var sut = new ContentRouteMatcher(contentRouteProvider);
            Assert.Same(routeA, sut.GetFor(contentTypeA));
            Assert.Empty(sut.GetFor(contentTypeB));
        }
    }
}
