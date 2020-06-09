using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Cloudy.CMS.ContentTypeSupport.GroupSupport
{
    [DebuggerDisplay("{Id}")]
    public class ContentTypeGroupDescriptor
    {
        public string Id { get; }
        public Type Type { get; }

        public ContentTypeGroupDescriptor(string id, Type type)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Type = type ?? throw new ArgumentNullException(nameof(type));
        }

        public override bool Equals(object obj)
        {
            var t = obj as ContentTypeDescriptor;

            if (t == null)
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
