using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Cloudy.CMS.UI.FormSupport.Controls.SelectSupport
{
    public class Item
    {
        public string Text { get; }
        public string Value { get; }
        public string Image { get; }
        public IDictionary<string, string> Metadata { get; }

        public Item(string text, string value, string image, IDictionary<string, string> metadata)
        {
            Text = text;
            Value = value;
            Image = image;
            Metadata = new ReadOnlyDictionary<string, string>(metadata);
        }
    }
}