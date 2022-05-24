/*
 * MIT License
 * 
 * Copyright (c) 2022 plexdata.de
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using NUnit.Framework;
using Plexdata.ArgumentParser.Attributes;
using Plexdata.ArgumentParser.Exceptions;
using Plexdata.ArgumentParser.Interfaces;
using Plexdata.ArgumentParser.Processors;
using System;
using System.Reflection;

namespace Plexdata.ArgumentParser.Tests.Processors
{
    [TestFixture]
    [TestOf(nameof(ArgumentProcessorSetting))]
    public class ArgumentProcessorSettingTests
    {
        [SetUp]
        public void SetUp()
        {
            ArgumentProcessorSettingTests.parameter = String.Empty;
            ArgumentProcessorSettingTests.argument = String.Empty;
            ArgumentProcessorSettingTests.delimiter = String.Empty;
        }

        [Test]
        [TestCase(typeof(PropertyInfo), null)]
        [TestCase(typeof(ParameterObjectAttribute), null)]
        [TestCase(typeof(PropertyInfo), false)]
        [TestCase(typeof(ParameterObjectAttribute), false)]
        [TestCase(typeof(PropertyInfo), true)]
        [TestCase(typeof(ParameterObjectAttribute), true)]
        public void Construction_InvalidArguments_ThrowsArgumentNullException(Type nullType, Boolean? enabled)
        {
            TestClassStandard candidate = new TestClassStandard();

            PropertyInfo property = candidate.GetType().GetRuntimeProperty("TestType");
            ParameterObjectAttribute attribute = (ParameterObjectAttribute)property.GetCustomAttribute(typeof(OptionParameterAttribute));

            property = nullType == typeof(PropertyInfo) ? null : property;
            attribute = nullType == typeof(ParameterObjectAttribute) ? null : attribute;

            if (enabled.HasValue)
            {
                Assert.Throws<ArgumentNullException>(() => { new ArgumentProcessorSetting(property, attribute, enabled.Value); });
            }
            else
            {
                Assert.Throws<ArgumentNullException>(() => { new ArgumentProcessorSetting(property, attribute); });
            }
        }

        [Test]
        [TestCase(typeof(TestClassStandard))]
        [TestCase(typeof(TestClassNullConverter))]
        [TestCase(typeof(TestClassConverterConstructorThrows))]
        [TestCase(typeof(TestClassWrongConverterWrongDataType))]
        [TestCase(typeof(TestClassWrongConverterNoGenericType))]
        [TestCase(typeof(TestClassWrongConverterWrongInterfaceType))]
        [TestCase(typeof(TestClassWrongConverterNoGenericInterface))]
        public void Construction_TestClassOfSpecificType_CustomConverterIsDisabled(Type type)
        {
            Object candidate = null;

            if (type == typeof(TestClassStandard)) { candidate = new TestClassStandard(); }
            else if (type == typeof(TestClassNullConverter)) { candidate = new TestClassNullConverter(); }
            else if (type == typeof(TestClassConverterConstructorThrows)) { candidate = new TestClassConverterConstructorThrows(); }
            else if (type == typeof(TestClassWrongConverterWrongDataType)) { candidate = new TestClassWrongConverterWrongDataType(); }
            else if (type == typeof(TestClassWrongConverterNoGenericType)) { candidate = new TestClassWrongConverterNoGenericType(); }
            else if (type == typeof(TestClassWrongConverterWrongInterfaceType)) { candidate = new TestClassWrongConverterWrongInterfaceType(); }
            else if (type == typeof(TestClassWrongConverterNoGenericInterface)) { candidate = new TestClassWrongConverterNoGenericInterface(); }

            PropertyInfo property = candidate.GetType().GetRuntimeProperty("TestType");
            ParameterObjectAttribute attribute = (ParameterObjectAttribute)property.GetCustomAttribute(typeof(OptionParameterAttribute));

            ArgumentProcessorSetting actual = new ArgumentProcessorSetting(property, attribute, true);

            Assert.That(actual.CustomConverter, Is.Null);
            Assert.That(actual.HasCustomConverter, Is.False);
        }

        [Test]
        public void Construction_TestClassConverter_CustomConverterIsEnabled()
        {
            TestClassConverter candidate = new TestClassConverter();

            PropertyInfo property = candidate.GetType().GetRuntimeProperty("TestType");
            ParameterObjectAttribute attribute = (ParameterObjectAttribute)property.GetCustomAttribute(typeof(OptionParameterAttribute));

            ArgumentProcessorSetting actual = new ArgumentProcessorSetting(property, attribute, true);

            Assert.That(actual.CustomConverter, Is.Not.Null);
            Assert.That(actual.HasCustomConverter, Is.True);
        }

        [Test]
        [TestCase(null)]
        [TestCase(true)]
        [TestCase(false)]
        public void InvokeCustomConverter_CustomConverterIsDisabled_ThrowsCustomConverterException(Boolean? enabled)
        {
            TestClassStandard candidate = new TestClassStandard();

            PropertyInfo property = candidate.GetType().GetRuntimeProperty("TestType");
            ParameterObjectAttribute attribute = (ParameterObjectAttribute)property.GetCustomAttribute(typeof(OptionParameterAttribute));

            ArgumentProcessorSetting actual = null;

            if (enabled.HasValue)
            {
                actual = new ArgumentProcessorSetting(property, attribute, enabled.Value);
            }
            else
            {
                actual = new ArgumentProcessorSetting(property, attribute);
            }

            Assert.That(() => actual.InvokeCustomConverter("parameter", "argument", "delimiter"), Throws.InstanceOf<CustomConverterException>());
        }

        [Test]
        [TestCase(typeof(TestClassInvokeConvertThrowsNotImplementedException))]
        [TestCase(typeof(TestClassInvokeConvertThrowsCustomConverterException))]
        public void InvokeCustomConverter_ConvertMethodInvocationThrows_ThrowsCustomConverterException(Type type)
        {
            Object candidate = null;

            if (type == typeof(TestClassInvokeConvertThrowsNotImplementedException)) { candidate = new TestClassInvokeConvertThrowsNotImplementedException(); }
            else if (type == typeof(TestClassInvokeConvertThrowsCustomConverterException)) { candidate = new TestClassInvokeConvertThrowsCustomConverterException(); }

            PropertyInfo property = candidate.GetType().GetRuntimeProperty("TestType");
            ParameterObjectAttribute attribute = (ParameterObjectAttribute)property.GetCustomAttribute(typeof(OptionParameterAttribute));

            ArgumentProcessorSetting actual = new ArgumentProcessorSetting(property, attribute, true);

            Assert.That(() => actual.InvokeCustomConverter("parameter", "argument", "delimiter"), Throws.InstanceOf<CustomConverterException>());
        }

        [Test]
        public void InvokeCustomConverter_ConvertMethodInvocation_ParametersAsExpected()
        {
            TestClassConverter candidate = new TestClassConverter();

            PropertyInfo property = candidate.GetType().GetRuntimeProperty("TestType");
            ParameterObjectAttribute attribute = (ParameterObjectAttribute)property.GetCustomAttribute(typeof(OptionParameterAttribute));

            ArgumentProcessorSetting actual = new ArgumentProcessorSetting(property, attribute, true);

            actual.InvokeCustomConverter("parameter", "argument", "delimiter");

            Assert.That(ArgumentProcessorSettingTests.parameter, Is.EqualTo("parameter"));
            Assert.That(ArgumentProcessorSettingTests.argument, Is.EqualTo("argument"));
            Assert.That(ArgumentProcessorSettingTests.delimiter, Is.EqualTo("delimiter"));
        }

        #region Test helpers

        private static String parameter = String.Empty;
        private static String argument = String.Empty;
        private static String delimiter = String.Empty;

        private struct TestType
        {
            public Int32 Count { get; set; }
            public String Name { get; set; }
        }

        private class TestTypeConverter : ICustomConverter<TestType>
        {
            public TestType Convert(String parameter, String argument, String delimiter)
            {
                ArgumentProcessorSettingTests.parameter = parameter;
                ArgumentProcessorSettingTests.argument = argument;
                ArgumentProcessorSettingTests.delimiter = delimiter;

                return new TestType();
            }
        }

        private class TestTypeConverterWithNotImplementedException : ICustomConverter<TestType>
        {
            public TestType Convert(String parameter, String argument, String delimiter)
            {
                throw new NotImplementedException();
            }
        }

        private class TestTypeConverterWithCustomConverterException : ICustomConverter<TestType>
        {
            public TestType Convert(String parameter, String argument, String delimiter)
            {
                throw new CustomConverterException("message");
            }
        }

        private class TestTypeConverterConstructorThrows : ICustomConverter<TestType>
        {
            public TestTypeConverterConstructorThrows()
            {
                throw new NotImplementedException();
            }

            public TestType Convert(String parameter, String argument, String delimiter)
            {
                throw new NotImplementedException();
            }
        }

        private class TestTypeConverterWrongDataType : ICustomConverter<Object>
        {
            public Object Convert(String parameter, String argument, String delimiter)
            {
                throw new NotImplementedException();
            }
        }

        private class TestTypeConverterWrongInterfaceType : IComparable<TestType>
        {
            public Int32 CompareTo(TestType other)
            {
                throw new NotImplementedException();
            }
        }

        private class TestTypeConverterNoGenericInterface : IDisposable
        {
            public void Dispose()
            {
                throw new NotImplementedException();
            }
        }

        private class TestClassStandard
        {
            [OptionParameter]
            public TestType TestType { get; set; }
        }

        private class TestClassConverter
        {
            [OptionParameter]
            [CustomConverter(typeof(TestTypeConverter))]
            public TestType TestType { get; set; }
        }

        private class TestClassNullConverter
        {
            [OptionParameter]
            [CustomConverter(null)]
            public TestType TestType { get; set; }
        }

        private class TestClassWrongConverterNoGenericType
        {
            [OptionParameter]
            [CustomConverter(typeof(Object))]
            public TestType TestType { get; set; }
        }

        private class TestClassWrongConverterWrongInterfaceType
        {
            [OptionParameter]
            [CustomConverter(typeof(TestTypeConverterWrongInterfaceType))]
            public TestType TestType { get; set; }
        }

        private class TestClassWrongConverterNoGenericInterface
        {
            [OptionParameter]
            [CustomConverter(typeof(TestTypeConverterNoGenericInterface))]
            public TestType TestType { get; set; }
        }

        private class TestClassWrongConverterWrongDataType
        {
            [OptionParameter]
            [CustomConverter(typeof(TestTypeConverterWrongDataType))]
            public TestType TestType { get; set; }
        }

        private class TestClassConverterConstructorThrows
        {
            [OptionParameter]
            [CustomConverter(typeof(TestTypeConverterConstructorThrows))]
            public TestType TestType { get; set; }
        }

        private class TestClassInvokeConvertThrowsNotImplementedException
        {
            [OptionParameter]
            [CustomConverter(typeof(TestTypeConverterWithNotImplementedException))]
            public TestType TestType { get; set; }
        }

        private class TestClassInvokeConvertThrowsCustomConverterException
        {
            [OptionParameter]
            [CustomConverter(typeof(TestTypeConverterWithCustomConverterException))]
            public TestType TestType { get; set; }
        }

        #endregion
    }
}
