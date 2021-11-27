using Cloudy.CMS.ContentSupport.Serialization;
using Cloudy.CMS.ContentTypeSupport;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Tests
{
    public class ContentJsonConverterTypeProviderTests
    {
        abstract class BaseClassA
        {

        }
        class ContentTypeA : BaseClassA
        {

        }
        interface InterfaceB
        {

        }
        class ContentTypeB : InterfaceB
        {

        }
        class ContentTypeC : InterfaceB
        {

        }

        [Fact]
        public void AddsBaseTypes()
        {
            var contentTypeProvider = Mock.Of<IContentTypeProvider>();
            Mock.Get(contentTypeProvider).Setup(p => p.GetAll()).Returns(new List<ContentTypeDescriptor> { new ContentTypeDescriptor("contentTypeA", typeof(ContentTypeA)) });

            var result = new ContentJsonConverterTypeProvider(contentTypeProvider).GetAll();

            Assert.Equal(new List<Type> { typeof(ContentTypeA), typeof(BaseClassA) }.AsReadOnly(), result);
        }

        [Fact]
        public void AddsInterfaces()
        {
            var contentTypeProvider = Mock.Of<IContentTypeProvider>();
            Mock.Get(contentTypeProvider).Setup(p => p.GetAll()).Returns(new List<ContentTypeDescriptor> { new ContentTypeDescriptor("contentTypeB", typeof(ContentTypeB)) });

            var result = new ContentJsonConverterTypeProvider(contentTypeProvider).GetAll();

            Assert.Equal(new List<Type> { typeof(ContentTypeB), typeof(InterfaceB) }.AsReadOnly(), result);
        }

        [Fact]
        public void HandlesDuplicateTypes()
        {
            var contentTypeProvider = Mock.Of<IContentTypeProvider>();
            Mock.Get(contentTypeProvider).Setup(p => p.GetAll()).Returns(new List<ContentTypeDescriptor> { new ContentTypeDescriptor("contentTypeB", typeof(ContentTypeB)), new ContentTypeDescriptor("contentTypeC", typeof(ContentTypeC)) });

            var result = new ContentJsonConverterTypeProvider(contentTypeProvider).GetAll();

            Assert.Equal(new List<Type> { typeof(ContentTypeB), typeof(InterfaceB), typeof(ContentTypeC) }.AsReadOnly(), result);
        }
    }
}
