using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.LanguageSupport
{
    public class LanguageDescriptor : IComparable
    {
        public string Id { get; }
        public string Name { get; }

        public LanguageDescriptor(string id, string name)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public override bool Equals(object obj)
        {
            return Id.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public int CompareTo(object obj)
        {
            var language = obj as LanguageDescriptor;

            if(language == null)
            {
                return 0;
            }

            return Id.CompareTo(language.Id);
        }
    }
}
