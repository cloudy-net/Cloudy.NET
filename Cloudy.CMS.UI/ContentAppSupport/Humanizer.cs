using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Cloudy.CMS.UI.ContentAppSupport
{
    public class Humanizer : IHumanizer
    {
        public string Humanize(string value)
        {
            value = Regex.Replace(value, @"([A-Z])([A-Z][a-z])", "$1 $2");
            value = Regex.Replace(value, @"([a-z])([A-Z])", "$1 $2");
            value = Regex.Replace(value, @"([a-z])([0-9])", "$1 $2");
            value = Regex.Replace(value, @"([0-9])([A-Z])", "$1 $2");
            value = Regex.Replace(value, @" [A-Z][a-z]", m => m.Value.ToLower());

            return value;
        }
    }
}
