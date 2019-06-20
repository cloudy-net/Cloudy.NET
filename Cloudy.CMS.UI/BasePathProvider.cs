using System;
using System.Collections.Generic;
using System.Text;

namespace Poetry.UI
{
    public class BasePathProvider : IBasePathProvider
    {
        public string BasePath { get; }

        public BasePathProvider(string basePath)
        {
            BasePath = basePath;
        }
    }
}
