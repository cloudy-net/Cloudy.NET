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
            var routeA = new ContentRouteDescriptor("template", new List<Type> { typeof(ClassA) });
            var routeB = new ContentRouteDescriptor("template", new List<Type> {  });
            var contentRouteProvider = Mock.Of<IContentRouteProvider>();
            Mock.Get(contentRouteProvider).Setup(p => p.GetAll()).Returns(new List<ContentRouteDescriptor> { routeA, routeB });
            
            var sut = new ContentRouteMatcher(contentRouteProvider);
            Assert.Equal(new List<ContentRouteDescriptor> { routeA }, sut.GetFor(typeof(ClassA)));
            Assert.Empty(sut.GetFor(typeof(ClassB)));
        }

        class ClassA
        {

        }

        class ClassB
        {

        }
    }
}
