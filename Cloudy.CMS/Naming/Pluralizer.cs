using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloudy.CMS.Naming
{
    public class Pluralizer : IPluralizer
    {
        public string Pluralize(string word)
        {
            string suffix = null;

            if(word.Contains(" <"))
            {
                var index = word.IndexOf(" <");
                suffix = word[index..];
                word = word[..index];
            }

            if (word.Any(c => char.IsDigit(c)))
            {
                return word + suffix;
            }

            if (word.EndsWith("ss"))
            {
                return word + "es" + suffix;
            }

            if (word.EndsWith('s'))
            {
                return word + suffix;
            }

            if (word.EndsWith('y'))
            {
                word = word.Substring(0, word.Length - 1) + "ie" + suffix;
            }

            word = word + "s";

            return word + suffix;
        }
    }
}
