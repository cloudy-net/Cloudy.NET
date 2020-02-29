using Cloudy.CMS.UI.FormSupport.UIHintSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloudy.CMS.UI.FormSupport.ControlSupport.MatchingSupport.UIHintControlMappingSupport
{
    public class UIHintControlMatchEvaluator : IUIHintControlMatchEvaluator
    {
        public bool IsMatch(UIHint uiHint, UIHintDefinition definition)
        {
            var minParameters = definition.Parameters.Where(p => !p.Optional).Count();
            var maxParameters = definition.Parameters.Count();

            if (uiHint.Parameters.Count < minParameters)
            {
                return false;
            }

            if (uiHint.Parameters.Count > maxParameters)
            {
                return false;
            }

            for (var i = 0; i < uiHint.Parameters.Count; i++)
            {
                var parameter = uiHint.Parameters[i];
                var parameterDefinition = definition.Parameters[i];

                if (parameterDefinition.Type == UIHintParameterType.Any)
                {
                    continue;
                }

                if (parameterDefinition.Type != parameter.Type)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
