using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.DocumentSupport
{
    public class DocumentDoesNotExistException : Exception
    {
        public DocumentDoesNotExistException(string container, string id) : base($"Document {id} in container {container} does not exist") { }
    }
}
