using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI.ContentAppSupport.ActionSupport
{
    public class ContentActionModuleCreator : IContentActionModuleCreator
    {
        IContentTypeProvider ContentTypeProvider { get; }

        public ContentActionModuleCreator(IContentTypeProvider contentTypeProvider)
        {
            ContentTypeProvider = contentTypeProvider;
        }

        public IEnumerable<ContentActionModuleDescriptor> Create()
        {
            var result = new List<ContentActionModuleDescriptor>();

            foreach(var contentType in ContentTypeProvider.GetAll())
            {
                foreach(ListActionsAttribute attribute in contentType.Type.GetCustomAttributes(typeof(ListActionsAttribute), true))
                {
                    result.Add(new ContentActionModuleDescriptor(contentType.Id, attribute.ModulePath));
                }
            }

            return result.AsReadOnly();
        }
    }
}
