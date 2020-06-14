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
        [Fact]
        public void ThrowsOnContentTypes()
        {
            var contentTypeA = new ContentTypeDescriptor("lorem", typeof(ContentTypeA), "container");
            var contentTypeProvider = Mock.Of<IContentTypeProvider>();
            Mock.Get(contentTypeProvider).Setup(p => p.GetAll()).Returns(new List<ContentTypeDescriptor> { contentTypeA });

            var formProvider = Mock.Of<IFormProvider>();
            Mock.Get(formProvider).Setup(p => p.GetAll()).Returns(new List<FormDescriptor> { });

            Assert.Throws<CannotInlineContentTypesException>(() => new PolymorphicFormFinder(contentTypeProvider, formProvider).FindFor(typeof(ContentTypeInterface)));
        }

        class ContentTypeA : ContentTypeInterface
        {
        }

        interface ContentTypeInterface
        {
        }

        [Fact]
        public void FindsForms()
        {
            var contentTypeProvider = Mock.Of<IContentTypeProvider>();
            Mock.Get(contentTypeProvider).Setup(p => p.GetAll()).Returns(new List<ContentTypeDescriptor> { });

            var formA = new FormDescriptor("lorem", typeof(FormA));
            var formB = new FormDescriptor("ipsum", typeof(FormB));
            var formProvider = Mock.Of<IFormProvider>();
            Mock.Get(formProvider).Setup(p => p.GetAll()).Returns(new List<FormDescriptor> { formA, formB });

            var result = new PolymorphicFormFinder(contentTypeProvider, formProvider).FindFor(typeof(FormInterface));

            Assert.Equal(new List<string> { "lorem" }, result);
        }

        class FormA : FormInterface
        {

        }

        interface FormInterface
        {
        }

        class FormB
        {
        }
    }
}
