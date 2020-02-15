using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI.ContentAppSupport.ListActionSupport
{
    public class ListActionModuleCreator : IListActionModuleCreator
    {
        IContentTypeProvider ContentTypeProvider { get; }

        public ListActionModuleCreator(IContentTypeProvider contentTypeProvider)
        {
            ContentTypeProvider = contentTypeProvider;
        }

        public IEnumerable<ListActionModuleDescriptor> Create()
        {
            var result = new List<ListActionModuleDescriptor>();

            foreach(var contentType in ContentTypeProvider.GetAll())
            {
                foreach(ListActionsAttribute attribute in contentType.Type.GetCustomAttributes(typeof(ListActionsAttribute), true))
                {
                    result.Add(new ListActionModuleDescriptor(contentType.Id, attribute.ModulePath));
                }
            }

            return result.AsReadOnly();
        }
    }
}
