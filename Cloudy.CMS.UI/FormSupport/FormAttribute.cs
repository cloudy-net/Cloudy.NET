using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI.FormSupport
{
    public class FormAttribute : Attribute
    {
        public string Id { get; }

        public FormAttribute(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new Exception($"Id must be provided when using [Form(...)]. How about {Guid.NewGuid()} ?");
            }

            Id = id;
        }
    }
}
