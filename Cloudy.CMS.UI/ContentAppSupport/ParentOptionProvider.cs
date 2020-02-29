using MongoDB.Driver;
using Cloudy.CMS.DocumentSupport;
using Cloudy.CMS.UI.FormSupport.Controls.DropdownControlSupport;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.ContentSupport;

namespace Cloudy.CMS.UI.ContentAppSupport
{
    [OptionProvider("parent")]
    public class ParentOptionProvider : IOptionProvider
    {
        IDocumentFinder DocumentFinder { get; }
        IContentTypeProvider ContentTypeProvider { get; }

        public ParentOptionProvider(IDocumentFinder documentFinder, IContentTypeProvider contentTypeProvider)
        {
            DocumentFinder = documentFinder;
            ContentTypeProvider = contentTypeProvider;
        }

        public IEnumerable<Option> GetAll()
        {
            var contentTypes = ContentTypeProvider.GetAll().Where(t => typeof(IHierarchical).IsAssignableFrom(t.Type));

            var documents = DocumentFinder.Find(ContainerConstants.Content).WhereIn<IContent, string>(x => x.ContentTypeId, contentTypes.Select(t => t.Id)).Select<IContent, string>(x => x.Id).Select<INameable, string>(x => x.Name).GetResultAsync().Result;

            var result = new List<Option>();

            result.Add(new Option("(root)", null));

            foreach(var document in documents)
            {
                var name = document.GlobalFacet.Interfaces.ContainsKey("INameable") &&
                    document.GlobalFacet.Interfaces["INameable"].Properties.ContainsKey("Name") &&
                    document.GlobalFacet.Interfaces["INameable"].Properties["Name"] is string ?
                    (string)document.GlobalFacet.Interfaces["INameable"].Properties["Name"] :
                    document.Id;
                var option = new Option(name, document.Id);

                result.Add(option);
            }

            return result.AsReadOnly();
        }
    }
}
