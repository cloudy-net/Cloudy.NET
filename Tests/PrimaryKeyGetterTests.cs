using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentTypeSupport;
using Moq;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xunit;

namespace Tests
{
    public class PrimaryKeyGetterTests
    {
        class MyType
        {
            public double PrimaryKey { get; set; }
        }

        [Fact]
        public void ConvertsConvertibleTypes()
        {
            var b = typeof(double);

            var contentType = new ContentTypeDescriptor("lorem", typeof(MyType));

            var contentTypeProvider = Mock.Of<IContentTypeProvider>();

            Mock.Get(contentTypeProvider).Setup(c => c.Get("lorem")).Returns(contentType);

            var primaryKeyPropertyGetter = Mock.Of<IPrimaryKeyPropertyGetter>();

            Mock.Get(primaryKeyPropertyGetter).Setup(p => p.GetFor(typeof(MyType))).Returns(new List<PropertyInfo> { typeof(MyType).GetProperty(nameof(MyType.PrimaryKey)) });

            var result = new PrimaryKeyConverter(contentTypeProvider, primaryKeyPropertyGetter).Convert(new List<object> { 10 }, "lorem");

            Assert.IsType<double>(result[0]);
        }
    }
}
