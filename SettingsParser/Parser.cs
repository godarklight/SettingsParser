using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace SettingsParser
{
    public class ConfigParser<T> where T : class
    {
        public T Settings { get; private set; }
        private string filePath;

        /// <summary>
        /// The parser's constructor.
        /// </summary>
        /// <param name="settings">A class property.</param>
        /// <param name="filePath">The path to the file the parser should load/write values from/to.</param>
        public ConfigParser(T settings, string filePath)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException("filePath");
            }

            this.Settings = settings;
            this.filePath = filePath;
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
        }

        #region Load Settings
        /// <summary>
        /// Loads the values from a file, specified in the parser's constructor.
        /// </summary>
        public void LoadSettings()
        {
            FieldInfo[] settingFields = typeof(T).GetFields();

            if (!File.Exists(filePath))
            {
                SaveSettings();
            }
            
            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    while (!sr.EndOfStream)
                    {
                        string currentLine = sr.ReadLine();
                        if (currentLine == null)
                        {
                            break;
                        }

                        string trimmedLine = currentLine.Trim();
                        if (String.IsNullOrEmpty(trimmedLine))
                        {
                            continue;
                        }

                        if (!trimmedLine.Contains("=") || trimmedLine.StartsWith("#"))
                        {
                            continue;
                        }

                        string currentKey = trimmedLine.Substring(0, trimmedLine.IndexOf("="));
                        string currentValue = trimmedLine.Substring(trimmedLine.IndexOf("=") + 1);

                        foreach (FieldInfo settingField in settingFields)
                        {
                            if (settingField.Name == currentKey)
                            {
                                if (settingField.FieldType == typeof(string))
                                {
                                    settingField.SetValue(Settings, currentValue);
                                }
                                if (settingField.FieldType == typeof(bool)) // We do not allow invalid values
                                {
                                    if (currentValue == "1" || currentValue.ToLower() == bool.TrueString.ToLower())
                                    {
                                        settingField.SetValue(Settings, true);
                                    }
                                    else if (currentValue == "0" || currentValue.ToLower() == bool.FalseString.ToLower())
                                    {
                                        settingField.SetValue(Settings, false);
                                    }
                                }
                                if (settingField.FieldType == typeof(double))
                                {
                                    double doubleValue;
                                    if (double.TryParse(currentValue, out doubleValue))
                                    {
                                        settingField.SetValue(Settings, doubleValue);
                                    }
                                }
                                if (settingField.FieldType == typeof(float))
                                {
                                    float floatValue;
                                    if (float.TryParse(currentValue, out floatValue))
                                    {
                                        settingField.SetValue(Settings, floatValue);
                                    }
                                }
                                if (settingField.FieldType == typeof(decimal))
                                {
                                    decimal decimalValue;
                                    if (decimal.TryParse(currentValue, out decimalValue))
                                    {
                                        settingField.SetValue(Settings, decimalValue);
                                    }
                                }
                                if (settingField.FieldType == typeof(short))
                                {
                                    short shortValue;
                                    if (short.TryParse(currentValue, out shortValue))
                                    {
                                        settingField.SetValue(Settings, shortValue);
                                    }
                                }
                                if (settingField.FieldType == typeof(int))
                                {
                                    int intValue;
                                    if (int.TryParse(currentValue, out intValue))
                                    {
                                        settingField.SetValue(Settings, intValue);
                                    }
                                }
                                if (settingField.FieldType == typeof(long))
                                {
                                    long longValue;
                                    if (long.TryParse(currentValue, out longValue))
                                    {
                                        settingField.SetValue(Settings, longValue);
                                    }
                                }
                                if (settingField.FieldType == typeof(uint))
                                {
                                    uint uintValue;
                                    if (uint.TryParse(currentValue, out uintValue))
                                    {
                                        settingField.SetValue(Settings, uintValue);
                                    }
                                }
                                if (settingField.FieldType == typeof(ulong))
                                {
                                    ulong ulongValue;
                                    if (ulong.TryParse(currentValue, out ulongValue))
                                    {
                                        settingField.SetValue(Settings, ulongValue);
                                    }
                                }
                                if (settingField.FieldType == typeof(ushort))
                                {
                                    ushort ushortValue;
                                    if (ushort.TryParse(currentValue, out ushortValue))
                                    {
                                        settingField.SetValue(Settings, ushortValue);
                                    }
                                }
                                if (settingField.FieldType.IsEnum)
                                {
                                    if (Enum.IsDefined(settingField.FieldType, currentValue))
                                    {
                                        object enumValue = Enum.Parse(settingField.FieldType, currentValue);
                                        settingField.SetValue(Settings, enumValue);
                                    }
                                    else
                                    {
                                        if (settingField.FieldType.GetEnumUnderlyingType() == typeof(int))
                                        {
                                            int intValue;
                                            if (int.TryParse(currentValue, out intValue))
                                            {
                                                if (Enum.IsDefined(settingField.FieldType, intValue))
                                                {
                                                    settingField.SetValue(Settings, intValue);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            SaveSettings();
        }
        #endregion
        #region Save Settings
        /// <summary>
        /// Saves the settings to a file, specified in the parser's constructor.
        /// </summary>
        public void SaveSettings()
        {
            FieldInfo[] settingFields = typeof(T).GetFields();
            if (File.Exists(filePath + ".tmp"))
            {
                File.Delete(filePath + ".tmp");
            }
            using (FileStream fs = new FileStream(filePath + ".tmp", FileMode.CreateNew))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine("# Lines starting with hashtags are ignored by the reader");
                    sw.WriteLine("# Setting file format: (key)=(value)");
                    sw.WriteLine("#");
                    sw.WriteLine("# Invalid values will be reset to default");
                    sw.WriteLine("#");
                    sw.WriteLine("");
                    foreach (FieldInfo settingField in settingFields)
                    {
                        object descriptionAttribute = settingField.GetCustomAttributes(typeof(DescriptionAttribute), true).FirstOrDefault();
                        string settingDescription = descriptionAttribute != null ? ((DescriptionAttribute)descriptionAttribute).Description : string.Empty;

                        if (!string.IsNullOrEmpty(settingDescription))
                        {
                            sw.WriteLine(string.Format("# {0} - {1}", settingField.Name, settingDescription));
                        }
                        if (settingField.FieldType == typeof(string))
                        {
                            sw.WriteLine(string.Format("{0}={1}", settingField.Name, settingField.GetValue(Settings)));
                        }
                        if (settingField.FieldType == typeof(int) || settingField.FieldType == typeof(uint) || settingField.FieldType == typeof(short) || settingField.FieldType == typeof(long) || settingField.FieldType == typeof(ushort) || settingField.FieldType == typeof(ulong) || settingField.FieldType == typeof(float) || settingField.FieldType == typeof(double) || settingField.FieldType == typeof(decimal) || settingField.FieldType == typeof(bool))
                        {
                            sw.WriteLine(string.Format("{0}={1}", settingField.Name, settingField.GetValue(Settings).ToString()));
                        }
                        if (settingField.FieldType.IsEnum)
                        {
                            sw.WriteLine("#");
                            sw.WriteLine("# Valid values are:");
                            foreach (object enumValue in settingField.FieldType.GetEnumValues())
                            {
                                sw.WriteLine(string.Format("#   {0}", enumValue.ToString()));
                            }
                            sw.WriteLine(string.Format("{0}={1}", settingField.Name, settingField.GetValue(Settings)));
                        }
                        sw.WriteLine("");
                    }
                }
            }
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            File.Move(filePath + ".tmp", filePath);
        }
        #endregion
    }
}
