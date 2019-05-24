using System;

namespace Cloudy.CMS.ContentSupport
{
    public class LanguageSpecificAttribute : Attribute
    {
        public override string ToString()
        {
            return "language-specific";
        }
    }
}