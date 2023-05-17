using System;
using System.Collections.Generic;

namespace Cloudy.NET.EntitySupport.Serialization
{
    public interface IContentJsonConverterTypeProvider
    {
        IEnumerable<Type> GetAll();
    }
}