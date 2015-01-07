using System;

namespace SettingsParser
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
