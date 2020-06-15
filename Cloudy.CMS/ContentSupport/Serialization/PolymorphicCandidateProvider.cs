using Cloudy.CMS.ComposableSupport;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Cloudy.CMS.ContentSupport.Serialization
{
    public class PolymorphicCandidateProvider : IPolymorphicCandidateProvider
    {
        IEnumerable<PolymorphicCandidateDescriptor> Candidates { get; }
        IDictionary<string, PolymorphicCandidateDescriptor> CandidatesById { get; }
        IDictionary<Type, PolymorphicCandidateDescriptor> CandidatesByType { get; }

        public PolymorphicCandidateProvider(IComposableProvider composableProvider)
        {
            Candidates = composableProvider.GetAll<IPolymorphicCandidateCreator>().SelectMany(c => c.Create()).ToList().AsReadOnly();
            CandidatesById = new ReadOnlyDictionary<string, PolymorphicCandidateDescriptor>(Candidates.ToDictionary(c => c.Id, c => c));
            CandidatesByType = new ReadOnlyDictionary<Type, PolymorphicCandidateDescriptor>(Candidates.ToDictionary(c => c.Type, c => c));

        }

        public PolymorphicCandidateDescriptor Get(string id)
        {
            if (!CandidatesById.ContainsKey(id))
            {
                return null;
            }

            return CandidatesById[id];
        }

        public PolymorphicCandidateDescriptor Get(Type type)
        {
            if (!CandidatesByType.ContainsKey(type))
            {
                return null;
            }

            return CandidatesByType[type];
        }

        public IEnumerable<PolymorphicCandidateDescriptor> GetAll()
        {
            return Candidates;
        }
    }
}
