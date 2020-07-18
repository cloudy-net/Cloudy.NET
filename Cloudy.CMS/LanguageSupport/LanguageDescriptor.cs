using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.LanguageSupport
{
    public class LanguageDescriptor : IComparable
    {
        public string Code { get; }
        public string Name { get; }

        public LanguageDescriptor(string code, string name)
        {
            Code = code ?? throw new ArgumentNullException(nameof(code));
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public override bool Equals(object obj)
        {
            return Code.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Code.GetHashCode();
        }

        public int CompareTo(object obj)
        {
            var language = obj as LanguageDescriptor;

            if(language == null)
            {
                return 0;
            }

            return Code.CompareTo(language.Code);
        }
    }
}
