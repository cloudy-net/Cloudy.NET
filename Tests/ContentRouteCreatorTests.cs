using Cloudy.NET.EntityTypeSupport;
using Cloudy.NET.Routing;
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
        [InlineData("lorem/{route:contentroute}", "lorem/{contentroute}", "EntityTypeA,EntityTypeB")]
        public void CreatesRoutes(string pattern, string resultingTemplate, string resultingTypes)
        {
            var entityTypeA = new EntityTypeDescriptor("sit", typeof(EntityTypeA));
            var entityTypeB = new EntityTypeDescriptor("dol", typeof(EntityTypeB));

            var entityTypeProvider = Mock.Of<IEntityTypeProvider>();
            Mock.Get(entityTypeProvider).Setup(p => p.GetAll()).Returns(new List<EntityTypeDescriptor> { entityTypeA, entityTypeB });

            var endpoint = new RouteEndpoint(async context => { }, RoutePatternFactory.Parse(pattern), 0, null, null);
            var dataSource = new CompositeEndpointDataSource(new List<EndpointDataSource> { new DefaultEndpointDataSource(endpoint) });

            var results = new ContentRouteCreator(Mock.Of<ILogger<ContentRouteCreator>>(), dataSource, entityTypeProvider).Create();

            if(resultingTemplate == null)
            {
                Assert.Empty(results);
                return;
            }

            Assert.Single(results);

            var result = results.Single();

            Assert.Equal(resultingTemplate, result.Template);
            Assert.Equal(resultingTypes, string.Join(",", result.Types.Select(t => t.Name)));
        }

        class EntityTypeA : InterfaceA
        {
        }

        class EntityTypeB
        {
        }

        class EntityTypeC
        {
        }

        private interface InterfaceA
        {
        }
    }
}
