using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.UI.ContentAppSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Tests
{
    public class NameExpressionParserTests
    {
        [Fact]
        public void ParsesSimpleGetter()
        {
            var result = new NameExpressionParser().Parse(typeof(MyContent));

            Assert.Equal(nameof(MyContent.MyProperty), result);
        }

        public class MyContent : INameable
        {
            public string MyProperty { get; set; }
            public string Name => MyProperty;
        }
    }
}
