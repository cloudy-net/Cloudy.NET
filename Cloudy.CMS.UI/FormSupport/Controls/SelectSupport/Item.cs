using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Cloudy.CMS.UI.FormSupport.Controls.SelectSupport
{
    public class Item
    {
        public string Text { get; }
        public string SubText { get; }
        public string Value { get; }
        public string Image { get; }
        public bool IsParent { get; }
        public bool IsSelectable { get; }

        public Item(string text, string subText, string value, string image, bool isParent, bool isSelectable = true)
        {
            Text = text;
            SubText = subText;
            Value = value;
            Image = image;
            IsParent = isParent;
            IsSelectable = isSelectable;
        }
    }
}