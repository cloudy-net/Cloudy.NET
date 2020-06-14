using Cloudy.CMS.ComponentSupport;
using Cloudy.CMS.ComposableSupport;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Cloudy.CMS.UI.FormSupport
{
    public class FormProvider : IFormProvider
    {
        IEnumerable<FormDescriptor> Forms { get; }
        IDictionary<string, FormDescriptor> FormsById { get; }
        IDictionary<Type, FormDescriptor> FormsByType { get; }

        public FormProvider(IComposableProvider composableProvider)
        {
            Forms = composableProvider.GetAll<IFormCreator>().SelectMany(c => c.CreateAll()).ToList().AsReadOnly();
            FormsById = new ReadOnlyDictionary<string, FormDescriptor>(Forms.ToDictionary(f => f.Id, f => f));
            FormsByType = new ReadOnlyDictionary<Type, FormDescriptor>(Forms.ToDictionary(f => f.Type, f => f));
        }

        public IEnumerable<FormDescriptor> GetAll()
        {
            return Forms;
        }

        public FormDescriptor Get(string id)
        {
            if (!FormsById.ContainsKey(id))
            {
                return null;
            }

            return FormsById[id];
        }

        public FormDescriptor Get(Type type)
        {
            return GetMostSpecificAssignableFrom(FormsByType, type);
        }

        public static FormDescriptor GetMostSpecificAssignableFrom(IDictionary<Type, FormDescriptor> forms, Type type)
        {
            if (forms.ContainsKey(type))
            {
                return forms[type];
            }

            var baseType = type.GetTypeInfo().BaseType;

            if (baseType == null)
            {
                return null;
            }

            return GetMostSpecificAssignableFrom(forms, baseType);
        }
    }
}
