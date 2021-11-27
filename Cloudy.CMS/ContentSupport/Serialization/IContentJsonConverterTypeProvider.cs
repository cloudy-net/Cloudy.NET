using System;
using System.Collections.Generic;

namespace Cloudy.CMS.ContentSupport.Serialization
{
    public interface IContentJsonConverterTypeProvider
    {
        IEnumerable<Type> GetAll();
    }
}