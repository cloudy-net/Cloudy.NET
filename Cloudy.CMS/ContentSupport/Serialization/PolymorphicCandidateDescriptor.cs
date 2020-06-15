using System;

namespace Cloudy.CMS.ContentSupport.Serialization
{
    public class PolymorphicCandidateDescriptor
    {
        public string Id { get; }
        public Type Type { get; }

        public PolymorphicCandidateDescriptor(string id, Type type)
        {
            Id = id;
            Type = type;
        }
    }
}