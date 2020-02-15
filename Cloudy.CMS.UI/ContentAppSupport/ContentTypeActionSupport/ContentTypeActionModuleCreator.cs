using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI.ContentAppSupport.ContentTypeActionSupport
{
    public class ContentTypeActionModuleCreator : IContentTypeActionModuleCreator
    {
        IContentTypeProvider ContentTypeProvider { get; }

        public ContentTypeActionModuleCreator(IContentTypeProvider contentTypeProvider)
        {
            ContentTypeProvider = contentTypeProvider;
        }

        public IEnumerable<ContentTypeActionModuleDescriptor> Create()
        {
            var result = new List<ContentTypeActionModuleDescriptor>();

            foreach(var contentType in ContentTypeProvider.GetAll())
            {
                foreach(ContentTypeActionsAttribute attribute in contentType.Type.GetCustomAttributes(typeof(ContentTypeActionsAttribute), true))
                {
                    result.Add(new ContentTypeActionModuleDescriptor(contentType.Id, attribute.ModulePath));
                }
            }

            return result.AsReadOnly();
        }
    }
}
