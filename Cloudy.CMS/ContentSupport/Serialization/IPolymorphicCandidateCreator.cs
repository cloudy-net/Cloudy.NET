using Cloudy.CMS.ComposableSupport;
using System.Collections.Generic;

namespace Cloudy.CMS.ContentSupport.Serialization
{
    public interface IPolymorphicCandidateCreator : IComposable
    {
        IEnumerable<PolymorphicCandidateDescriptor> Create();
    }
}