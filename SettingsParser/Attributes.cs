using System;

namespace SettingsParser.Attributes
{
    public class DescriptionAttribute : Attribute
    {
        public string Description;
        public DescriptionAttribute(string Description)
        {
            this.Description = Description;
        }
    }
}
