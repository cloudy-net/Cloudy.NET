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

            //        if (PluralizationServiceUtil.TryInflectOnSuffixInWord(word,
            //new List<string>() { "trix" },
            //(s) => s.Remove(s.Length - 1, 1) + "ces", this.Culture, out newword))
            //        {
            //            return newword;
            //        }

            //        if (PluralizationServiceUtil.TryInflectOnSuffixInWord(word,
            //            new List<string>() { "eau", "ieu" },
            //            (s) => s + "x", this.Culture, out newword))
            //        {
            //            return newword;
            //        }

            //        if (this._wordsEndingWithInxAnxYnxPluralizationService.ExistsInFirst(word))
            //        {
            //            return this._wordsEndingWithInxAnxYnxPluralizationService.GetSecondValue(word);
            //        }

            //        // [cs]h and ss that take es as plural form
            //        if (PluralizationServiceUtil.TryInflectOnSuffixInWord(word, new List<string>() { "ch", "sh", "ss" }, (s) => s + "es", this.Culture, out newword))
            //        {
            //            return newword;
            //        }

            //        // f, fe that take ves as plural form
            //        if (PluralizationServiceUtil.TryInflectOnSuffixInWord(word,
            //            new List<string>() { "alf", "elf", "olf", "eaf", "arf" },
            //            (s) => s.EndsWith("deaf", true, this.Culture) ? s : s.Remove(s.Length - 1, 1) + "ves", this.Culture, out newword))
            //        {
            //            return newword;
            //        }

            //        if (PluralizationServiceUtil.TryInflectOnSuffixInWord(word,
            //            new List<string>() { "nife", "life", "wife" },
            //            (s) => s.Remove(s.Length - 2, 2) + "ves", this.Culture, out newword))
            //        {
            //            return newword;
            //        }

            //        // y takes ys as plural form if preceded by a vowel, but ies if preceded by a consonant, e.g. stays, skies
            //        if (PluralizationServiceUtil.TryInflectOnSuffixInWord(word,
            //            new List<string>() { "ay", "ey", "iy", "oy", "uy" },
            //            (s) => s + "s", this.Culture, out newword))
            //        {
            //            return newword;
            //        }

            //        // 

            //        if (word.EndsWith("y", true, this.Culture))
            //        {
            //            return word.Remove(word.Length - 1, 1) + "ies";
            //        }

            //        // handle some of the words o -> os, and [vowel]o -> os, and the rest are o->oes
            //        if (this._oSuffixPluralizationService.ExistsInFirst(word))
            //        {
            //            return this._oSuffixPluralizationService.GetSecondValue(word);
            //        }

            //        if (PluralizationServiceUtil.TryInflectOnSuffixInWord(word,
            //            new List<string>() { "ao", "eo", "io", "oo", "uo" },
            //            (s) => s + "s", this.Culture, out newword))
            //        {
            //            return newword;
            //        }

            //        if (word.EndsWith("o", true, this.Culture) || word.EndsWith("s", true, this.Culture))
            //        {
            //            return word + "es";
            //        }

            //        if (word.EndsWith("x", true, this.Culture))
            //        {
            //            return word + "es";
            //        }

            // cats, bags, hats, speakers

            if (word.EndsWith('s'))
            {
                return word;
            }

            return word + "s";

        }
    }
}
