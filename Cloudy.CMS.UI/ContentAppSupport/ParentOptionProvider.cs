using Cloudy.CMS.DocumentSupport;
using Cloudy.CMS.UI.FormSupport.Controls.DropdownControlSupport;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentSupport.Serialization;

namespace Cloudy.CMS.UI.ContentAppSupport
{
    [OptionProvider("parent")]
    public class ParentOptionProvider : IOptionProvider
    {
        IDocumentFinder DocumentFinder { get; }
        IContentTypeProvider ContentTypeProvider { get; }
        IContentDeserializer ContentDeserializer { get; }

        public ParentOptionProvider(IDocumentFinder documentFinder, IContentTypeProvider contentTypeProvider, IContentDeserializer contentDeserializer)
        {
            DocumentFinder = documentFinder;
            ContentTypeProvider = contentTypeProvider;
            ContentDeserializer = contentDeserializer;
        }

        public IEnumerable<Option> GetAll()
        {
            var contentTypes = ContentTypeProvider.GetAll().Where(t => typeof(IHierarchical).IsAssignableFrom(t.Type));

            var result = new List<Option>();

            foreach (var contentType in contentTypes)
            {
                var documents = DocumentFinder.Find(contentType.Container).WhereEquals<IContent, string>(x => x.ContentTypeId, contentType.Id).GetResultAsync().Result.ToList();

                foreach (var document in documents)
                {
                    var content = ContentDeserializer.Deserialize(document, contentType, DocumentLanguageConstants.Global);

                    result.Add(new Option((content as INameable).Name ?? content.Id, content.Id));
                }
            }

            result = result.OrderBy(i => i.Value).ToList();

            result.Insert(0, new Option("(root)", null));

            return result.AsReadOnly();
        }
    }
}
