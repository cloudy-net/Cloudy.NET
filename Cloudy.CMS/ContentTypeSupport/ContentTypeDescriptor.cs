using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.ContentTypeSupport
{
    [DebuggerDisplay("{Id}")]
    public class ContentTypeDescriptor
    {
        public string Id { get; }
        public Type Type { get; }
        public string Container { get; }

        public ContentTypeDescriptor(string id, Type type, string container)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Type = type ?? throw new ArgumentNullException(nameof(type));
            Container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public override bool Equals(object obj)
        {
            var t = obj as ContentTypeDescriptor;

            if(t == null)
            {
                return false;
            }

            return Id == t.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
