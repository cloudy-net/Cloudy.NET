using Cloudy.CMS.UI.FormSupport.UIHintSupport.ParserSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Tests
{
    public class UIHintDefinitionParserTests
    {
        [Fact]
        public void ParsesOptionalParameters()
        {
            var result = new UIHintDefinitionParser().Parse("lorem(ipsum?)");

            Assert.Single(result.Parameters);
            Assert.Equal("ipsum", result.Parameters.Single().Id);
            Assert.True(result.Parameters.Single().Optional);
        }
    }
}
