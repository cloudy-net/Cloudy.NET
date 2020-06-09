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
            var contentTypeA = new ContentTypeDescriptor("ipsum", typeof(ContentTypeA), "dolor");
            var contentTypeProvider = Mock.Of<IContentTypeProvider>();
            Mock.Get(contentTypeProvider).Setup(p => p.GetAll()).Returns(new List<ContentTypeDescriptor> { contentTypeA });

            var contentTypeGroupA = new ContentTypeGroupDescriptor("lorem", typeof(ContentTypeGroupA));
            var contentTypeGroupProvider = Mock.Of<IContentTypeGroupProvider>();
            Mock.Get(contentTypeGroupProvider).Setup(p => p.GetAll()).Returns(new List<ContentTypeGroupDescriptor> { contentTypeGroupA });

            var result = new ContentTypeGroupMatcher(contentTypeProvider, contentTypeGroupProvider).GetContentTypesFor("lorem");

            Assert.Equal(new List<ContentTypeDescriptor> { contentTypeA }, result);
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

        public class ContentTypeA : ContentTypeGroupA
        {

        }
    }
}
