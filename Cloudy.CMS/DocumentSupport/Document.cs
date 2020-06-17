using Cloudy.CMS.ContentSupport;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Cloudy.CMS.DocumentSupport
{
    [DebuggerDisplay("{DebugDisplay()}")]
    public class Document
    {
        public string Id { get; set; }
        public DocumentFacet GlobalFacet { get; set; }
        public IDictionary<string, DocumentFacet> LanguageFacets { get; set; }

        public static Document CreateFrom(string id, DocumentFacet globalFacet, IDictionary<string, DocumentFacet> languageFacets)
        {
            return new Document
            {
                Id = id,
                GlobalFacet = globalFacet,
                LanguageFacets = new ReadOnlyDictionary<string, DocumentFacet>(languageFacets),
            };
        }

        string DebugDisplay()
        {
            if(GlobalFacet.Interfaces.ContainsKey(nameof(INameable)) && GlobalFacet.Interfaces[nameof(INameable)].Properties[nameof(INameable.Name)] != null)
            {
                return $"{GlobalFacet.Interfaces[nameof(INameable)].Properties[nameof(INameable.Name)]} ({Id}) [{GlobalFacet.Interfaces[nameof(IContent)].Properties[nameof(IContent.ContentTypeId)]}]";
            }

            return $"{Id} [{GlobalFacet.Interfaces[nameof(IContent)].Properties[nameof(IContent.ContentTypeId)]}]";
        }
    }
}
