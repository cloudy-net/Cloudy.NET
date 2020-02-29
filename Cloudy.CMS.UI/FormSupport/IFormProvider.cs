using System;
using System.Collections.Generic;

namespace Cloudy.CMS.UI.FormSupport
{
    public interface IFormProvider
    {
        IEnumerable<FormDescriptor> GetAll();
    }
}