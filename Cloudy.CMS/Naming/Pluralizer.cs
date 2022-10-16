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

            return word + "s";

        }
    }
}
