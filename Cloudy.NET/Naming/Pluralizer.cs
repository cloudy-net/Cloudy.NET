using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloudy.NET.Naming
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

            if (word.Any(char.IsDigit))
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

            if(word == "Persons")
            {
                word = "People";
            }

            return word + suffix;
        }
    }
}
