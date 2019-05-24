using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Cloudy.CMS.ContentControllerSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloudy.CMS.AspNetCore.ContentControllerSupport
{
    public class ControllerProvider : IControllerProvider
    {
        IEnumerable<Type> Controllers { get; }

        public ControllerProvider(ApplicationPartManager partManager)
        {
            var feature = new ControllerFeature();
            partManager.PopulateFeature(feature);
            Controllers = feature.Controllers.Select(c => c.AsType());
        }

        public IEnumerable<Type> GetAll()
        {
            return Controllers.ToList().AsReadOnly();
        }
    }
}
