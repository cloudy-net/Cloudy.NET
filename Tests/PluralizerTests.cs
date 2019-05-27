using Cloudy.CMS.UI.ContentAppSupport;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Tests
{
    public class PluralizerTests
    {
        [Theory]
        [InlineData("abf322af-4a6b-4afd-be29-65187b97dfbd", "abf322af-4a6b-4afd-be29-65187b97dfbd")]
        [InlineData("article", "articles")]
        public void Pluralize(string input, string expected)
        {
            var result = new Pluralizer().Pluralize(input);

            Assert.Equal(expected, result);
        }
    }
}
