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
            var result = new NameExpressionParser().Parse(typeof(ContentWithNameToOtherProperty));

            Assert.Equal(nameof(ContentWithNameToOtherProperty.OtherProperty), result);
        }

        public class ContentWithNameToOtherProperty : INameable
        {
            public string OtherProperty { get; set; }
            public string Name => OtherProperty;
        }
        [Fact]
        public void ParsesExplicitImplementation()
        {
            var result = new NameExpressionParser().Parse(typeof(ContentWithExplicitNameToOtherProperty));

            Assert.Equal(nameof(ContentWithExplicitNameToOtherProperty.OtherProperty), result);
        }

        public class ContentWithExplicitNameToOtherProperty : INameable
        {
            public string OtherProperty { get; set; }
            string INameable.Name => OtherProperty;
        }

        [Fact]
        public void ParsesNameIfNormalProperty()
        {
            var result = new NameExpressionParser().Parse(typeof(ContentWithNormalProperty));

            Assert.Equal(nameof(ContentWithNormalProperty.Name), result);
        }

        public class ContentWithNormalProperty : INameable
        {
            public string Name { get; set; }
        }
    }
}
