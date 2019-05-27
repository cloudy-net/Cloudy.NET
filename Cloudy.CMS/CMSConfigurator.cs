using Cloudy.CMS.DocumentSupport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS
{
    public class CMSConfigurator
    {
        public void SetDatabase(string databaseName)
        {
            DatabaseNameProvider.DatabaseName = databaseName;
        }
    }
}
