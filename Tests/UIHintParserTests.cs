using Cloudy.CMS.UI.FormSupport.UIHintSupport;
using Cloudy.CMS.UI.FormSupport.UIHintSupport.ParserSupport;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Tests
{
    public class UIHintParserTests
    {
        [Fact]
        public void WithoutParenthesis()
        {
            var result = new UIHintParser(Mock.Of<IUIHintParameterValueParser>()).Parse("lorem");

            Assert.Equal("lorem", result.Id);
            Assert.Empty(result.Parameters);
        }

        [Fact]
        public void WithParenthesis()
        {
            var result = new UIHintParser(Mock.Of<IUIHintParameterValueParser>()).Parse("lorem()");

            Assert.Equal("lorem", result.Id);
            Assert.Empty(result.Parameters);
        }

        [Fact]
        public void OneParameter()
        {
            var param = new UIHintParameterValue("ipsum");
            var parameterValueParser = Mock.Of<IUIHintParameterValueParser>();
            Mock.Get(parameterValueParser).Setup(p => p.Parse(It.IsAny<IParser>())).Returns<IParser>(p => {
                if (p.IsThenSkip("ipsum"))
                {
                    return param;
                }

                throw new Exception("Unknown input");
            });
            var result = new UIHintParser(parameterValueParser).Parse("lorem(ipsum)");

            Assert.Equal("lorem", result.Id);
            Assert.Single(result.Parameters);
            Assert.Same(param, result.Parameters.Single());
        }

        [Fact]
        public void TwoSimpleExpressionParameter()
        {
            var param1 = new UIHintParameterValue("ipsum");
            var param2 = new UIHintParameterValue("dolor");
            var parameterValueParser = Mock.Of<IUIHintParameterValueParser>();
            Mock.Get(parameterValueParser).Setup(p => p.Parse(It.IsAny<IParser>())).Returns<IParser>(p => {
                if (p.IsThenSkip("ipsum"))
                {
                    return param1;
                }

                if (p.IsThenSkip("dolor"))
                {
                    return param2;
                }

                throw new Exception($"Unknown input at {p}");
            });

            var result = new UIHintParser(parameterValueParser).Parse("lorem(ipsum, dolor)");

            Assert.Equal("lorem", result.Id);
            Assert.Equal(2, result.Parameters.Count);
        }
    }
}
