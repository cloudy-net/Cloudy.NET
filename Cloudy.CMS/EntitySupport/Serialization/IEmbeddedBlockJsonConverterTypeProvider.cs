using System;
using System.Collections.Generic;

namespace Cloudy.CMS.EntitySupport.Serialization
{
    public interface IContentJsonConverterTypeProvider
    {
        IEnumerable<Type> GetAll();
    }
}