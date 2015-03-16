using System;
using SettingsParser;
using System.IO;
using System.Reflection;
using System.ComponentModel;
using System.Collections.Generic;

namespace SettingsParserTester
{
    enum TestEnum : int
    {
        TEST1 = 0,
        TEST2 = 1,
        TEST3 = 2
    }
    class TestClass
    {
        [Description("This is a test string.\nTest2\nTest3")]
        public string testString;
        public bool testBool;
        public int testInt;
        public double testDouble;
        public decimal testDecimal;
        public float testFloat;
        public uint testUInt;
        public long testLong;
        public ulong testULong;
        public short testShort;
        public ushort testUShort;
        public TestEnum testEnum;
        public List<string> testStringList;
        [Description("This is a test int list")]
        public List<int> testIntList;
        public string[] testStringArray;
        [Description("This is a test double array")]
        public double[] testDoubleArray;
    }

    class SettingsParserTester
    {
        public static void Main()
        {
            string settingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test.txt");

            if (File.Exists(settingsPath))
            {
                File.Delete(settingsPath);
            }

            ConfigParser<TestClass> settingsParserSave = new ConfigParser<TestClass>(GetFilledTestClass(), settingsPath);
            settingsParserSave.SaveSettings();

            ConfigParser<TestClass> settingsParserLoad = new ConfigParser<TestClass>(new TestClass(), settingsPath);
            Console.WriteLine("Loading settings");
            settingsParserLoad.LoadSettings();

            //Display settings
            FieldInfo[] settingsFields = typeof(TestClass).GetFields();
            foreach (FieldInfo settingField in settingsFields)
            {
                object originalObject = settingField.GetValue(settingsParserSave.Settings);
                object settingObject = settingField.GetValue(settingsParserLoad.Settings);
                if (settingObject == null)
                {
                    Console.WriteLine(settingField.Name + " IS NULL");
                    continue;
                }
                if (settingField.FieldType == typeof(List<string>))
                {
                    List<string> list = (List<string>)settingObject;
                    Console.WriteLine(settingField.Name + " LIST START");
                    foreach (string str in list)
                    {
                        Console.WriteLine("ELEMENT: " + str);
                    }
                    Console.WriteLine(settingField.Name + " LIST END");
                    continue;
                }
                if (settingField.FieldType == typeof(List<int>))
                {
                    List<int> list = (List<int>)settingObject;
                    Console.WriteLine(settingField.Name + " LIST START");
                    foreach (int i in list)
                    {
                        Console.WriteLine("ELEMENT: " + i);
                    }
                    Console.WriteLine(settingField.Name + " LIST END");
                    continue;
                }
                if (settingField.FieldType == typeof(string[]))
                {
                    string[] arr = (string[])settingObject;
                    Console.WriteLine(settingField.Name + " ARRAY START");
                    foreach (string str in arr)
                    {
                        Console.WriteLine("ELEMENT: " + str);
                    }
                    Console.WriteLine(settingField.Name + " ARRAY END");
                    continue;
                }
                if (settingField.FieldType == typeof(double[]))
                {
                    double[] arr = (double[])settingObject;
                    Console.WriteLine(settingField.Name + " ARRAY START");
                    foreach (double d in arr)
                    {
                        Console.WriteLine("ELEMENT: " + d);
                    }
                    Console.WriteLine(settingField.Name + " ARRAY END");
                    continue;
                }
                Console.WriteLine(settingField.Name + "=" + settingObject);
            }
            Console.ReadKey();
        }

        public static TestClass GetFilledTestClass()
        {
            TestClass retVal = new TestClass();
            retVal.testString = "The quick brown fox jumped over the lazy dog";
            retVal.testBool = false;
            retVal.testInt = 120;
            retVal.testDouble = 1.35;
            retVal.testDecimal = 1.36m;
            retVal.testFloat = 1.37f;
            retVal.testUInt = 4294967290;
            retVal.testLong = 9223372036854775807;
            retVal.testULong = 9223372036854775808;
            retVal.testShort = 32767;
            retVal.testUShort = 65535;
            retVal.testEnum = TestEnum.TEST1;
            retVal.testStringList = new List<string> { "abc", "def", "ghi", "\t\r\n\tabc" };
            retVal.testIntList = new List<int> { 1, 2, 3, 4 };
            retVal.testStringArray = new string[] { "zyx", "w,v,u", "\\,tsrq,\\", "\t\r\n\tpon" };
            retVal.testDoubleArray = new double[] { .5, .25, .1, double.NegativeInfinity };
            return retVal;
        }
    }
}
