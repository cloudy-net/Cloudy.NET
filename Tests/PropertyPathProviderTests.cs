using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.DocumentSupport;
using Cloudy.CMS.DocumentSupport.CacheSupport;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Xunit;

namespace Tests
{
    public class PropertyPathProviderTests
    {
        [Fact]
        public void ThrowsOnParameterCountBeingOtherThanOne()
        {
            var sut = new DocumentPropertyPathProvider(Mock.Of<ICoreInterfaceProvider>());
            Assert.Throws<OnlyOneParameterAllowedInExpressionException>(() => sut.GetFor((Expression<Func<string>>)(() => string.Empty)));
            Assert.Throws<OnlyOneParameterAllowedInExpressionException>(() => sut.GetFor((Expression<Func<object, object, string>>)((d1, d2) => string.Empty)));
            Assert.Throws<OnlyOneParameterAllowedInExpressionException>(() => sut.GetFor((Expression<Func<object, object, object, string>>)((d1, d2, d3) => string.Empty)));
        }

        [Fact]
        public void CoreInterfaceGlobalProperty()
        {
            Expression<Func<MyCoreInterface, string>> expression = d => d.Lorem;

            var coreInterfaceProvider = Mock.Of<ICoreInterfaceProvider>();

            Mock.Get(coreInterfaceProvider).Setup(p => p.GetFor(typeof(MyCoreInterface))).Returns(new CoreInterfaceDescriptor("MyCoreInterfaceId", typeof(MyCoreInterface), new List<PropertyDefinitionDescriptor> { new PropertyDefinitionDescriptor(nameof(MyCoreInterface.Lorem), typeof(string), o => o, (o, v) => { }, Enumerable.Empty<object>()) }));

            var result = new DocumentPropertyPathProvider(coreInterfaceProvider).GetFor(expression);

            Assert.Equal("GlobalFacet.Interfaces.MyCoreInterfaceId.Properties.Lorem", result);
        }

        public interface MyCoreInterface
        {
            string Lorem { get; set; }
        }
    }
}
