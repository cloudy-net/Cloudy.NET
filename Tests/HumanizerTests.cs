using Cloudy.CMS.UI.ContentAppSupport;
using System;
using Xunit;

namespace Tests
{
    public class HumanizerTests
    {
        [Theory]
        [InlineData("UIHintCMSTest", "UI hint CMS test")]
        [InlineData("Test1Two", "Test 1 two")]
        public void Humanize(string input, string expected)
        {
            var result = new Humanizer().Humanize(input);

            Assert.Equal(expected, result);
        }
    }
}
