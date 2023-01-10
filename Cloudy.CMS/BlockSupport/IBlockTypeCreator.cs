using System;
using System.Collections.Generic;

namespace Cloudy.CMS.BlockSupport
{
    public interface IBlockTypeCreator
    {
        IEnumerable<Type> Create();
    }
}