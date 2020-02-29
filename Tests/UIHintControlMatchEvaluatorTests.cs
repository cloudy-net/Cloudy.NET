using Cloudy.CMS.UI.FormSupport.ControlSupport.MatchingSupport.UIHintControlMappingSupport;
using Cloudy.CMS.UI.FormSupport.UIHintSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Tests
{
    public class UIHintControlMatchEvaluatorTests
    {
        [Theory]
        [InlineData(0, false)]
        [InlineData(1, true)]
        [InlineData(2, true)]
        [InlineData(3, false)]
        public void AllowsOptionalParameters(int parameterCount, bool expectedResult)
        {
            var definition = new UIHintDefinition("lorem", new List<UIHintParameterDefinition> { 
                new UIHintParameterDefinition("ipsum", UIHintParameterType.Any, false),
                new UIHintParameterDefinition("dolor", UIHintParameterType.Any, true),
            });

            var parameters = new List<UIHintParameterValue>();

            for (var i = 0; i < parameterCount; i++)
            {
                parameters.Add(new UIHintParameterValue(1));
            }

            Assert.Equal(expectedResult, new UIHintControlMatchEvaluator().IsMatch(new UIHint("lorem", parameters), definition));

        }
    }
}
