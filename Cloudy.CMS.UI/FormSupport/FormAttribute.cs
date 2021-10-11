using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI.FormSupport
{
    public class FormAttribute : Attribute
    {
        public string Id { get; }

        [Obsolete("You need to supply an Id - use the parameterless constructor only to get a suggestion")]
        public FormAttribute() : this(null) { }

        public FormAttribute(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new Exception($"Id must be provided when using [Form(...)]. How about `{Guid.NewGuid()}` ?");
            }

            Id = id;
        }
    }
}
