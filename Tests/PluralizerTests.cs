using Cloudy.CMS.Naming;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Tests
{
    public class PluralizerTests
    {
        [Theory]
        [InlineData("abf322af", "abf322af")]
        [InlineData("article", "articles")]
        [InlineData("class", "classes")]
        [InlineData("library", "libraries")]
        [InlineData("book <something>", "books <something>")]
        [InlineData("page <123>", "pages <123>")]
        public void Pluralize(string input, string expected)
        {
            var result = new Pluralizer().Pluralize(input);

            Assert.Equal(expected, result);
        }
    }
}
