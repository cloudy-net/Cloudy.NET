using Cloudy.CMS.ComponentSupport;
using Cloudy.CMS.ComposableSupport;
using System;
using System.Collections.Generic;

namespace Cloudy.CMS.UI.FormSupport
{
    public interface IFormCreator : IComposable
    {
        IEnumerable<FormDescriptor> CreateAll();
    }
}