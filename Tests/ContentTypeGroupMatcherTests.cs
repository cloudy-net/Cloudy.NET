using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.ContentTypeSupport.GroupSupport;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Tests
{
    public class ContentTypeGroupMatcherTests
    {
        [Fact]
        public void FindsContentTypes()
        {
            var typeA = new ContentTypeDescriptor("ipsum", typeof(ContentTypeA), "dolor");
            var typeB = new ContentTypeDescriptor("sit", typeof(ContentTypeB), "amet");
            var typeProvider = Mock.Of<IContentTypeProvider>();
            Mock.Get(typeProvider).Setup(p => p.GetAll()).Returns(new List<ContentTypeDescriptor> { typeA, typeB });

            var groupA = new ContentTypeGroupDescriptor("lorem", typeof(ContentTypeGroupA));
            var groupB = new ContentTypeGroupDescriptor("adipiscing", typeof(ContentTypeGroupB));
            var groupProvider = Mock.Of<IContentTypeGroupProvider>();
            Mock.Get(groupProvider).Setup(p => p.Get("lorem")).Returns(groupA);
            Mock.Get(groupProvider).Setup(p => p.Get("adipiscing")).Returns(groupB);

            var sut = new ContentTypeGroupMatcher(typeProvider, groupProvider);

            Assert.Equal(new List<ContentTypeDescriptor> { typeA }, sut.GetContentTypesFor("lorem"));
            Assert.Equal(new List<ContentTypeDescriptor> { typeA, typeB }, sut.GetContentTypesFor("adipiscing"));
        }

        [Fact]
        public void FindsContentTypeGroups()
        {
            var contentTypeA = new ContentTypeDescriptor("ipsum", typeof(ContentTypeA), "dolor");
            var contentTypeProvider = Mock.Of<IContentTypeProvider>();
            Mock.Get(contentTypeProvider).Setup(p => p.Get("ipsum")).Returns(contentTypeA);

            var contentTypeGroupA = new ContentTypeGroupDescriptor("lorem", typeof(ContentTypeGroupA));
            var contentTypeGroupProvider = Mock.Of<IContentTypeGroupProvider>();
            Mock.Get(contentTypeGroupProvider).Setup(p => p.GetAll()).Returns(new List<ContentTypeGroupDescriptor> { contentTypeGroupA });

            var result = new ContentTypeGroupMatcher(contentTypeProvider, contentTypeGroupProvider).GetContentTypeGroupsFor("ipsum");

            Assert.Equal(new List<ContentTypeGroupDescriptor> { contentTypeGroupA }, result);
        }

        public interface ContentTypeGroupA
        {

        }

        public class ContentTypeA : ContentTypeGroupA, ContentTypeGroupB
        {

        }

        public interface ContentTypeGroupB
        {

        }

        public class ContentTypeB : ContentTypeGroupB
        {

        }
    }
}
