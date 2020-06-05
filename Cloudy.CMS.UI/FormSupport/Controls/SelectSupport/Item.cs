namespace Cloudy.CMS.UI.FormSupport.Controls.SelectSupport
{
    public class Item
    {
        public string Text { get; }
        public string Value { get; }
        public string Image { get; }

        public Item(string text, string value, string image)
        {
            Text = text;
            Value = value;
            Image = image;
        }
    }
}