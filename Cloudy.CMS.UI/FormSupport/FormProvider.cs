using Cloudy.CMS.ComponentSupport;
using Cloudy.CMS.ComposableSupport;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Cloudy.CMS.UI.FormSupport
{
    public class FormProvider : IFormProvider
    {
        IEnumerable<FormDescriptor> Forms { get; }

        public FormProvider(IComposableProvider composableProvider)
        {
            Forms = composableProvider.GetAll<IFormCreator>().SelectMany(c => c.CreateAll()).ToList().AsReadOnly();
        }

        public IEnumerable<FormDescriptor> GetAll()
        {
            return Forms;
        }
    }
}
