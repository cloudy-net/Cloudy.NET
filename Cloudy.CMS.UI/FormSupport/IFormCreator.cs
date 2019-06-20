using Poetry.ComponentSupport;
using Poetry.ComposableSupport;
using System;
using System.Collections.Generic;

namespace Poetry.UI.FormSupport
{
    public interface IFormCreator : IComposable
    {
        IEnumerable<FormDescriptor> CreateAll();
    }
}