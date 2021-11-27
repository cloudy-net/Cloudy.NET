using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.UI.ContentAppSupport;
using Cloudy.CMS.UI.FormSupport;
using Cloudy.CMS.UI.FormSupport.ControlSupport.MatchingSupport.PolymorphicControlMappingSupport;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Tests
{
    public class PolymorphicFormFinderTests
    {
        class ContentTypeA : ContentTypeInterface
        {
        }
        class ContentTypeB
        {
        }

        interface ContentTypeInterface
        {
        }

        [Fact]
        public void FindsContentTypeByInterface()
        {
            var contentTypeProvider = Mock.Of<IContentTypeProvider>();
            var a = new ContentTypeDescriptor("lorem", typeof(ContentTypeA));
            var b = new ContentTypeDescriptor("ipsum", typeof(ContentTypeB));
            Mock.Get(contentTypeProvider).Setup(p => p.GetAll()).Returns(new List<ContentTypeDescriptor> { a, b });

            var result = new PolymorphicFormFinder(contentTypeProvider).FindFor(typeof(ContentTypeInterface));

            Assert.Equal(new List<string> { "lorem" }, result);
        }
    }
}
