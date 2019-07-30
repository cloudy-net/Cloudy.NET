using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.DocumentSupport
{
    public class DocumentAlreadyExistsException : Exception
    {
        public DocumentAlreadyExistsException(string container, string id) : base($"Document {id} in container {container} already exists") { }
    }
}
