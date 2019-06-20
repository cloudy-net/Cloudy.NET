using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Poetry.UI.PortalSupport
{
    public interface IMainPageGenerator
    {
        void Generate(Stream write);
    }
}
