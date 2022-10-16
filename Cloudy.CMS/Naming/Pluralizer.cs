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
            if (word.Any(c => char.IsDigit(c)))
            {
                return word;
            }

            if (word.EndsWith('s'))
            {
                return word;
            }

            if (word.EndsWith('y'))
            {
                word = word.Substring(0, word.Length - 1) + "ie";
            }

            word = word + "s";

            return word;
        }
    }
}
