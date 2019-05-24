using Poetry.ComponentSupport;
using Poetry.DependencyInjectionSupport;
using Poetry.UI.DataTableSupport.BackendSupport;
using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Cloudy.CMS.UI.ContentAppSupport
{
    public class ContentDataTableBackendCreator : IBackendCreator
    {
        IContentTypeProvider ContentTypeRepository { get; }
        IInstantiator Instantiator { get; }

        public ContentDataTableBackendCreator(IContentTypeProvider contentTypeRepository, IInstantiator instantiator)
        {
            ContentTypeRepository = contentTypeRepository;
            Instantiator = instantiator;
        }

        public IDictionary<string, IBackend> CreateAll()
        {
            return new ReadOnlyDictionary<string, IBackend>(ContentTypeRepository.GetAll().ToDictionary(t => $"Cloudy.CMS.ContentList[type={t.Id}]", t => (IBackend)Instantiator.Instantiate(typeof(ContentDataTableBackend<>).MakeGenericType(t.Type))));
        }
    }
}
