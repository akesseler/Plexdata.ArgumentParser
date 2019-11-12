/*
 * MIT License
 * 
 * Copyright (c) 2019 plexdata.de
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

using Moq;
using NUnit.Framework;
using Plexdata.ArgumentParser.Exceptions;
using Plexdata.ArgumentParser.Extensions;
using Plexdata.ArgumentParser.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Plexdata.ArgumentParser.Tests
{
    [TestFixture]
    [TestOf(nameof(CustomConverterExtension))]
    public class CustomConverterExtensionTests
    {
        private Mock<ICustomConverter<Int16>> mockConverter1;
        private Mock<ICustomConverter<Int32>> mockConverter2;

        [SetUp]
        public void SetUp()
        {
            this.mockConverter1 = new Mock<ICustomConverter<Int16>>();
            this.mockConverter2 = new Mock<ICustomConverter<Int32>>();
        }

        [TearDown]
        public void TearDown()
        {
            this.ClearConverters();
        }

        [Test]
        public void AddConverter_ConverterIsNull_ConverterCountIsZero()
        {
            ICustomConverter<Int16> nullConverter = null;

            nullConverter.AddConverter();

            Assert.That(this.GetConverterCount(), Is.Zero);
        }

        [Test]
        [TestCase(0, 0)]
        [TestCase(1, 1)]
        [TestCase(2, 1)]
        [TestCase(3, 1)]
        public void AddConverter_MultipleAddingOfSameConverter_ConverterCountAsExpected(Int32 actual, Int32 expected)
        {
            for (Int32 index = 0; index < actual; index++)
            {
                this.mockConverter1.Object.AddConverter();
            }

            Assert.That(this.GetConverterCount(), Is.EqualTo(expected));
        }

        [Test]
        public void RemoveConverter_ConverterIsNull_ConverterCountIsZero()
        {
            ICustomConverter<Int16> nullConverter = null;

            nullConverter.RemoveConverter();

            Assert.That(this.GetConverterCount(), Is.Zero);
        }

        [Test]
        [TestCase(0, 0)]
        [TestCase(1, 0)]
        [TestCase(2, 0)]
        [TestCase(3, 0)]
        public void RemoveConverter_MultipleAddingOfSameConverter_ConverterCountAsExpected(Int32 actual, Int32 expected)
        {
            for (Int32 index = 0; index < actual; index++)
            {
                this.mockConverter1.Object.AddConverter();
            }

            this.mockConverter1.Object.RemoveConverter();

            Assert.That(this.GetConverterCount(), Is.EqualTo(expected));
        }

        [Test]
        [TestCase(typeof(Int16), true)]
        [TestCase(typeof(Int32), true)]
        [TestCase(typeof(Int64), false)]
        public void HasConverter_CheckConverterForType_ConverterExistsAsExpected(Type actual, Boolean expected)
        {
            this.mockConverter1.Object.AddConverter();
            this.mockConverter2.Object.AddConverter();

            Assert.That(actual.HasConverter(), Is.EqualTo(expected));
        }

        [Test]
        [TestCase("string")]
        [TestCase(typeof(Int64))]
        public void InvokeConverter_ObjectIsInvalid_ThrowsCustomConverterException(Object actual)
        {
            this.mockConverter1.Object.AddConverter();
            this.mockConverter2.Object.AddConverter();

            Assert.That(() => actual.InvokeConverter("parameter", "argument", "delimiter"), Throws.InstanceOf<CustomConverterException>());
        }

        [Test]
        [Category("IntegrationTest")]
        public void InvokeConverter_InvokingConvert_ConvertWasCalledOnce()
        {
            this.mockConverter1.Object.AddConverter();
            this.mockConverter2.Object.AddConverter();

            typeof(Int16).InvokeConverter("parameter", "argument", "delimiter");

            this.mockConverter1.Verify(x => x.Convert("parameter", "argument", "delimiter"), Times.Once);
        }

        [Test]
        [Category("IntegrationTest")]
        public void InvokeConverter_InvokingConvertThrowsCustomConverterException_ThrowsCustomConverterException()
        {
            this.mockConverter1.Object.AddConverter();
            this.mockConverter2.Object.AddConverter();

            this.mockConverter1.Setup(x => x.Convert(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>())).Throws(new CustomConverterException("message"));

            Assert.That(() => typeof(Int16).InvokeConverter("parameter", "argument", "delimiter"), Throws.InstanceOf<CustomConverterException>());
        }

        [Test]
        [Category("IntegrationTest")]
        public void InvokeConverter_InvokingConvertThrowsFormatException_ThrowsCustomConverterException()
        {
            this.mockConverter1.Object.AddConverter();
            this.mockConverter2.Object.AddConverter();

            this.mockConverter1.Setup(x => x.Convert(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>())).Throws(new FormatException());

            Assert.That(() => typeof(Int16).InvokeConverter("parameter", "argument", "delimiter"), Throws.InstanceOf<CustomConverterException>());
        }

        [Test]
        [Category("IntegrationTest")]
        public void InvokeConverter_InvokingConvertReturnsObject_ResultAsExpected()
        {
            this.mockConverter1.Object.AddConverter();
            this.mockConverter2.Object.AddConverter();

            this.mockConverter1.Setup(x => x.Convert(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>())).Returns(42);

            Object actual = typeof(Int16).InvokeConverter("parameter", "argument", "delimiter");

            Assert.That(actual, Is.EqualTo(42));
        }

        private void ClearConverters()
        {
            (typeof(CustomConverterExtension)
                .GetField("converters", BindingFlags.GetField | BindingFlags.NonPublic | BindingFlags.Static)
                .GetValue(null) as IDictionary<String, Object>).Clear();

        }

        private Int32 GetConverterCount()
        {
            return (typeof(CustomConverterExtension)
                .GetField("converters", BindingFlags.GetField | BindingFlags.NonPublic | BindingFlags.Static)
                .GetValue(null) as IDictionary<String, Object>).Count;
        }
    }
}
