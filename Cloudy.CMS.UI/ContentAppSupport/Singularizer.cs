using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloudy.CMS.UI.ContentAppSupport
{
    public class Singularizer : ISingularizer
    {
        public string Singularize(string word)
        {
            if (word.Any(c => char.IsDigit(c)))
            {
                return word;
            }

            if (word.EndsWith('s'))
            {
                return word.Substring(0, word.Length - 1);
            }

            return word;
        }
    }
}
