namespace Cloudy.CMS.UI.FormSupport.Controls.DropdownControlSupport
{
    public class Option
    {
        public string Text { get; }
        public string Value { get; }

        public Option(string text, string value)
        {
            Text = text;
            Value = value;
        }
    }
}