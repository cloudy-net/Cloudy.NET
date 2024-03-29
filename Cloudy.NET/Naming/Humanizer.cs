﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Cloudy.NET.Naming
{
    public class Humanizer : IHumanizer
    {
        public string Humanize(string value)
        {
            if(value == string.Empty)
            {
                return value;
            }

            value = value[..1].ToUpper() + value[1..]; // uppercase first letter
            value = value.Replace("-", " "); // split words on hyphen/dash (kebab case)
            value = value.Replace("_", " "); // split words on underscore (snake case)
            value = value.Replace("<", " <"); // insert space after lesserthan (used for generic types<like,this>)
            value = Regex.Replace(value, @"([A-Z])([A-Z][a-z])", "$1 $2"); // separate uppercase letters and succeeding capitalized word (eg. UIHint)
            value = Regex.Replace(value, @"([a-z])([A-Z])", "$1 $2"); // separate lowercase letter and uppercase letter (camel case)
            value = Regex.Replace(value, @"([a-z])([0-9])", "$1 $2"); // separate lowercase letter and succeeding digit (lorem0)
            value = Regex.Replace(value, @"([0-9])([A-Z])", "$1 $2"); // separate digit and succeeding uppercase letter (0Text)
            value = Regex.Replace(value, @" [A-Z][a-z]", m => m.Value.ToLower()); // lowercase all capital initial letters after first word
            value = value.Replace(",", ", "); // insert space after comma (used for generic types<like,this>)

            return value;
        }
    }
}
