using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Tests
{
    public class ContentRouterTests
    {
        [Fact]
        public void RootPageWithNullUrlSegment()
        {
            var root = new Page
            {
                Id = "root",
                UrlSegment = null,
            };

            var result = new ContentRouter(new RootContentRouter(new TestContentSegmentRouter(root)), new TestRoutableRootContentProvider(root)).RouteContent(new List<string> { }, null);

            Assert.Same(root, result);
        }

        [Fact]
        public void RootPageWithNonNullUrlSegment()
        {
            var root = new Page
            {
                UrlSegment = "lorem",
            };

            var result = new ContentRouter(new RootContentRouter(new TestContentSegmentRouter(root)), new TestRoutableRootContentProvider(root)).RouteContent(new List<string> { "lorem" }, null);

            Assert.Same(root, result);
        }

        [Fact]
        public void PageUnderRootPageWithNonNullUrlSegment()
        {
            var root = new Page
            {
                Id = "root",
                UrlSegment = "lorem",
            };

            var page = new Page
            {
                ParentId = root.Id,
                UrlSegment = "ipsum",
            };

            var result = new ContentRouter(new RootContentRouter(new TestContentSegmentRouter(root, page)), new TestRoutableRootContentProvider(root)).RouteContent(new List<string> { "lorem", "ipsum" }, null);

            Assert.Same(page, result);
        }

        [Fact]
        public void PageUnderRootPageWithNullUrlSegment()
        {
            var root = new Page
            {
                Id = "root",
                UrlSegment = null,
            };

            var page = new Page
            {
                ParentId = root.Id,
                UrlSegment = "ipsum",
            };

            var result = new ContentRouter(new RootContentRouter(new TestContentSegmentRouter(root, page)), new TestRoutableRootContentProvider(root)).RouteContent(new List<string> { "ipsum" }, null);

            Assert.Same(page, result);
        }

        public class TestRoutableRootContentProvider : IRoutableRootContentProvider
        {
            IEnumerable<IContent> Content { get; }

            public TestRoutableRootContentProvider(params IContent[] content)
            {
                Content = content;
            }

            public IEnumerable<IContent> GetAll()
            {
                return Content;
            }
        }

        public class TestContentSegmentRouter : IContentSegmentRouter
        {
            IEnumerable<Page> Pages { get; }

            public TestContentSegmentRouter(params Page[] pages)
            {
                Pages = pages;
            }

            public IContent RouteContentSegment(string parentId, string segment, string language)
            {
                return Pages.FirstOrDefault(p => p.ParentId == parentId && p.UrlSegment == segment);
            }
        }

        public class Page : IContent, IHierarchical, IRoutable
        {
            public string Id { get; set; }
            public string ContentTypeId { get; set; }
            public string ParentId { get; set; }
            public string UrlSegment { get; set; }
        }
    }
}
