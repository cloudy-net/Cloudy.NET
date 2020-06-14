using Cloudy.CMS.UI.FormSupport.ControlSupport.MatchingSupport;
using Cloudy.CMS.UI.FormSupport.ControlSupport.MatchingSupport.UIHintControlMappingSupport;
using Cloudy.CMS.UI.FormSupport.ControlSupport.MatchingSupport.TypeControlMappingSupport;
using Cloudy.CMS.UI.FormSupport.ControlSupport.MatchingSupport.UIHintControlMappingSupport;
using Cloudy.CMS.UI.FormSupport.UIHintSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Cloudy.CMS.UI.FormSupport.ControlSupport.MatchingSupport.PolymorphicControlMappingSupport;

namespace Cloudy.CMS.UI.FormSupport.ControlSupport.MatchingSupport
{
    public class ControlMatcher : IControlMatcher
    {
        ILogger<ControlMatcher> Logger { get; }
        IUIHintControlMatcher UIHintControlMatcher { get; }
        ITypeControlMatcher TypeControlMatcher { get; }
        IPolymorphicFormFinder PolymorphicFormFinder { get; }
        IDictionary<string, ControlDescriptor> Controls { get; }

        public ControlMatcher(ILogger<ControlMatcher> logger, IUIHintControlMatcher uiHintControlMatcher, ITypeControlMatcher typeControlMatcher, IPolymorphicFormFinder polymorphicFormFinder, IControlProvider controlProvider)
        {
            Logger = logger;
            TypeControlMatcher = typeControlMatcher;
            UIHintControlMatcher = uiHintControlMatcher;
            PolymorphicFormFinder = polymorphicFormFinder;
            Controls = controlProvider.GetAll().ToDictionary(c => c.Id, c => c);
        }

        public IControlMatch GetFor(Type type, IEnumerable<UIHint> uiHints)
        {
            foreach (var uiHint in uiHints)
            {
                var match = UIHintControlMatcher.GetFor(uiHint);

                if (Logger.IsEnabled(LogLevel.Information))
                {
                    Logger.LogInformation($"Match for {JsonConvert.SerializeObject(uiHint, Formatting.None)}: {JsonConvert.SerializeObject(match, Formatting.None)}");
                }

                if(match != null)
                {
                    return match;
                }
            }

            {
                var match = TypeControlMatcher.GetFor(type);

                if (match != null)
                {
                    return match;
                }
            }

            foreach (var uiHint in uiHints)
            {
                if (uiHint.Parameters.Any())
                {
                    continue;
                }

                if (Controls.ContainsKey(uiHint.Id)) {
                    return new UIHintControlMatch(uiHint.Id, uiHint.Id, new Dictionary<string, object>());
                }
            }

            if (type.IsInterface)
            {
                return new PolymorphicControlMatch("polymorphic-form", PolymorphicFormFinder.FindFor(type).ToList().AsReadOnly());
            }

            return null;
        }
    }
}
