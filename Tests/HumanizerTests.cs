using Cloudy.CMS.UI.ContentAppSupport;
using System;
using Xunit;

namespace Tests
{
    public class HumanizerTests
    {
        [Theory]
        [InlineData("515130cc-32be-4005-8b90-b356f9b90428", "515130cc-32be-4005-8b90-b356f9b90428")]
        [InlineData("UIHintCMSTest", "UI hint CMS test")]
        [InlineData("kebab-case", "Kebab case")]
        public void Humanize(string input, string expected)
        {
            var result = new Humanizer().Humanize(input);

            Assert.Equal(expected, result);
        }
    }
}
