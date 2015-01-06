﻿using System;
using SettingsParser;
using SettingsParser.Attributes;
using System.IO;
using System.Reflection;

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
        public string testString = "The quick brown fox jumped over the lazy dog";
        public bool testBool = false;
        public int testInt = 120;
        public double testDouble = 1.35;
        public decimal testDecimal = 1.36m;
        public float testFloat = 1.37f;
        public uint testUInt = 4294967290;
        public long testLong = 9223372036854775807;
        public ulong testULong = 9223372036854775808;
        public short testShort = 32767;
        public ushort testUShort = 65535;
        public TestEnum testEnum = TestEnum.TEST1;
    }

    class SettingsParserTester
    {
        public static void Main()
        {
            SettingsParser<TestClass> settingsParser = new SettingsParser<TestClass>(new TestClass(), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test.txt"));
            Console.WriteLine("Loading settings");
            settingsParser.LoadSettings();
            FieldInfo[] settingsFields = typeof(TestClass).GetFields();
            foreach (FieldInfo settingField in settingsFields)
            {
                Console.WriteLine(settingField.Name + "=" + settingField.GetValue(new TestClass()));
            }
            Console.ReadKey();
        }
    }
}