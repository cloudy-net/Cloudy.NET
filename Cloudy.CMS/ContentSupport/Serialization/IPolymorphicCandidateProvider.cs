using System;
using System.Collections.Generic;

namespace Cloudy.CMS.ContentSupport.Serialization
{
    public interface IPolymorphicCandidateProvider
    {
        IEnumerable<PolymorphicCandidateDescriptor> GetAll();
        PolymorphicCandidateDescriptor Get(string id);
        PolymorphicCandidateDescriptor Get(Type type);
    }
}