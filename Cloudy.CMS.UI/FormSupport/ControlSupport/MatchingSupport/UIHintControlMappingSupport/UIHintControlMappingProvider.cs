using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloudy.CMS.UI.FormSupport.ControlSupport.MatchingSupport.UIHintControlMappingSupport
{
    public class UIHintControlMappingProvider : IUIHintControlMappingProvider
    {
        IDictionary<string, IEnumerable<UIHintControlMapping>> Values { get; }

        public UIHintControlMappingProvider(IUIHintControlMappingCreator uIHintControlMappingCreator)
        {
            Values = uIHintControlMappingCreator.Create()
                .GroupBy(m => m.UIHintDefinition.Id)
                .ToDictionary(m => m.Key, m => (IEnumerable<UIHintControlMapping>)m);
        }

        public IEnumerable<UIHintControlMapping> GetFor(string uiHintId)
        {
            if (!Values.ContainsKey(uiHintId))
            {
                return Enumerable.Empty<UIHintControlMapping>();
            }

            return Values[uiHintId];
        }
    }
}
