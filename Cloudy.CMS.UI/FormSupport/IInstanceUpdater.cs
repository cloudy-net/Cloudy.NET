using Cloudy.CMS.ContentTypeSupport;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Cloudy.CMS.UI.FormSupport
{
    public interface IInstanceUpdater
    {
        void Update(ContentTypeDescriptor contentType, IEnumerable<string> primaryKeyNames, object instance, IFormCollection form);
    }
}