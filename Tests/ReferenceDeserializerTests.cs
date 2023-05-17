using Cloudy.NET.EntitySupport.PrimaryKey;
using Cloudy.NET.EntitySupport.Reference;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class ReferenceDeserializerTests
    {
        [Fact]
        public void DeserializesStringArray()
        {
            var primaryKeyPropertyGetter = Mock.Of<IPrimaryKeyPropertyGetter>();
            Mock.Get(primaryKeyPropertyGetter).Setup(p => p.GetFor(It.IsAny<Type>())).Returns(new List<PropertyInfo> { typeof(MyClass).GetProperty(nameof(MyClass.Property)) });

            var value = "[\"lorem\"]";
            var expected = new List<string> { "lorem" };
            var actual = new ReferenceDeserializer(primaryKeyPropertyGetter, Mock.Of<ILogger<ReferenceDeserializer>>()).Get(typeof(object), value, false);

            Assert.Equal(expected, actual);
        }

        public class MyClass
        {
            public string Property { get; set; }
        }
    }
}
