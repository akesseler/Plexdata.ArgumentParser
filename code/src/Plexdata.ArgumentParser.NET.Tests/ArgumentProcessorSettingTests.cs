/*
 * MIT License
 * 
 * Copyright (c) 2018 plexdata.de
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
using Plexdata.ArgumentParser.Processors;
using System;
using System.Globalization;
using System.Reflection;

namespace Plexdata.ArgumentParser.Tests
{
    [TestFixture]
    [TestOf(nameof(ArgumentProcessorSetting))]
    public class ArgumentProcessorSettingTests
    {
        private class TestInfo : PropertyInfo
        {
            public TestInfo() : base() { }

            public override Type PropertyType => throw new NotImplementedException();

            public override PropertyAttributes Attributes => throw new NotImplementedException();

            public override bool CanRead => throw new NotImplementedException();

            public override bool CanWrite => throw new NotImplementedException();

            public override string Name => throw new NotImplementedException();

            public override Type DeclaringType => throw new NotImplementedException();

            public override Type ReflectedType => throw new NotImplementedException();

            public override MethodInfo[] GetAccessors(bool nonPublic)
            {
                throw new NotImplementedException();
            }

            public override object[] GetCustomAttributes(bool inherit)
            {
                throw new NotImplementedException();
            }

            public override object[] GetCustomAttributes(Type attributeType, bool inherit)
            {
                throw new NotImplementedException();
            }

            public override MethodInfo GetGetMethod(bool nonPublic)
            {
                throw new NotImplementedException();
            }

            public override ParameterInfo[] GetIndexParameters()
            {
                throw new NotImplementedException();
            }

            public override MethodInfo GetSetMethod(bool nonPublic)
            {
                throw new NotImplementedException();
            }

            public override object GetValue(object obj, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
            {
                throw new NotImplementedException();
            }

            public override bool IsDefined(Type attributeType, bool inherit)
            {
                throw new NotImplementedException();
            }

            public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        private class TestAttr : ParameterObjectAttribute
        {
            public TestAttr() : base() { }
        }

        [Test]
        [TestCase(null, null)]
        [TestCase(typeof(TestInfo), null)]
        [TestCase(null, typeof(TestAttr))]
        public void Construction_NullArguments_ThrowsException(Type testInfoType, Type testAttrType)
        {
            TestInfo testInfo = testInfoType != null ? new TestInfo() : null;
            TestAttr testAttr = testAttrType != null ? new TestAttr() : null;
            Assert.Throws<ArgumentNullException>(() => { new ArgumentProcessorSetting(testInfo, testAttr); });
        }
    }
}
