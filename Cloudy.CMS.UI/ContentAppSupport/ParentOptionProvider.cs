using MongoDB.Driver;
using Cloudy.CMS.DocumentSupport;
using Poetry.UI.FormSupport.Controls.DropdownControlSupport;
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
        IContainerProvider ContainerProvider { get; }
        IContentTypeProvider ContentTypeProvider { get; }

        public ParentOptionProvider(IContainerProvider containerProvider, IContentTypeProvider contentTypeProvider)
        {
            ContainerProvider = containerProvider;
            ContentTypeProvider = contentTypeProvider;
        }

        public IEnumerable<Option> GetAll()
        {
            var contentTypes = ContentTypeProvider.GetAll().Where(t => typeof(IHierarchical).IsAssignableFrom(t.Type));

            var documents = ContainerProvider.Get(ContainerConstants.Content).FindSync(
                Builders<Document>.Filter.In(new StringFieldDefinition<Document, string>("GlobalFacet.Interfaces.IContent.Properties.ContentTypeId"), contentTypes.Select(t => t.Id)),
                new FindOptions<Document, Document>
                {
                    Projection =
                        Builders<Document>.Projection
                        .Include(new StringFieldDefinition<Document, string>("GlobalFacet.Interfaces.INameable.Properties.Name"))
                        .Include(new StringFieldDefinition<Document, string>("Id"))
                }
            )
                .ToList();

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
